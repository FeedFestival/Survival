using System;
using UnityEngine;
using UnityEngine.UI;
using b = Assets.Scripts.Utils.style_base;

namespace Assets.Scripts.Utils
{
    public static class style_utils
    {
        public static float GetPercent(float value, float percent)
        {
            return (value / 100f) * percent;
        }

        public static float GetValuePercent(float value, float maxValue)
        {
            return (value * 100f) / maxValue;
        }

        public static float XPercent(float percent)
        {
            return (b.ScreenWidth / 100f) * percent;
        }
        public static float YPercent(float percent)
        {
            return (b.ScreenHeight / 100f) * percent;
        }

        public static void BootstrapSize(value width, value height, div div)
        {
            var size = b.getParentFreeWidth(div.parent);
            width = b.getValue(width, size);

            // setup the paddings
            div.padding.left().floating = b.getFValue(div.padding.left(), width.floating);
            div.padding.right().floating = b.getFValue(div.padding.right(), width.floating);

            size = b.getParentFreeHeight(div.parent);
            height = b.getValue(height, size);

            div.padding.top().floating = b.getFValue(div.padding.top(), height.floating);
            div.padding.bottom().floating = b.getFValue(div.padding.bottom(), height.floating);

            b.SetFixedSize(width.floating, height.floating, div.element);
        }

        public static void SetPosition(float x, float y, Button button)
        {
            b.SetPosition(x, y, button.GetComponent<RectTransform>());
        }

        public static void SetPosition(float x, float y, Image image)
        {
            b.SetPosition(x, y, image.GetComponent<RectTransform>());
        }

        public static void SetAnchor(AnchorType at, RectTransform rt)
        {
            b.SetAnchor(at, rt);
        }

        public static void SetAnchor(AnchorType at, Button button)
        {
            b.SetAnchor(at, button.GetComponent<RectTransform>());
        }

        public static void SetAnchor(AnchorType at, Image image)
        {
            b.SetAnchor(at, image.GetComponent<RectTransform>());
        }

        public static void getDivs(Transform parent, div div)
        {
            if (div.ignoreChildren)
                return;

            // if it has a class
            if (div._class != _class.none)
                return;

            var i = 0;
            foreach (Transform child in parent)
            {
                // TODO: full functionality for borders
                if (child.gameObject.name == "border-top-2")
                {
                    div.border = child.GetComponent<RectTransform>();
                }

                if (style_base.isDiv(child) == false)
                    continue;
                
                var s = child.GetComponent<style>();

                if (s == null)
                    continue;

                s.Refresh();

                s.Div.childIndex = i;
                s.Div.parent = div;


                getDivs(child, s.Div);

                div.children.Add(s.Div);

                i++;
            }
        }

        public static void buildDiv(div div)
        {
            if (div.element.gameObject.activeSelf == false)
            {
                div.element.sizeDelta = new Vector2(0,0);
                return;
            }
            if (b.isBody(div.elementName) == false)
                build(div);
            if (div.children.Count > 0)
                foreach (div d in div.children)
                {
                    buildDiv(d);
                }
        }

        private static void build(div div)
        {
            if (div.type == divType.div)
            {
                BootstrapSize(div.width,
                    div.height,
                    div);

                setPosition(ref div);
            }
            else if (div.type == divType.label)
            {
                div.element.anchoredPosition = new Vector3(0f, 0f, 0f);
            }
            else if (div.type == divType.button)
            {
                BootstrapSize(div.width,
                    div.height,
                    div);

                div.element.anchoredPosition3D = new Vector3(0f, 0f, -2f);
                div.element.anchoredPosition = new Vector2(0f, 0f);
            }

            if (div.border != null)
            {
                // TODO: full functionality for borders
                div.border.sizeDelta = new Vector3(GetPercent(div.parent.element.sizeDelta.x, 100f), 24f);
                div.border.localPosition = new Vector2(0f, 2f);
            }

            if (div._class != _class.none)
            {
                style_class.buildClass(div);
            }
        }

        public static void setPosition(ref div div)
        {
            var parentFreeSize = b.getParentFreeWidth(div.parent);

            var elSize = div.element.sizeDelta.x + div.margin.horizontal();

            if (parentFreeSize >= elSize)
            {
                if (div.margin.left().auto && div.margin.right().auto) // it means we center this boot horizontally.
                {
                    var remainingSpace = parentFreeSize - elSize;
                    div.margin.left().floating = remainingSpace / 2;
                }
                else
                {
                    div.margin.left().floating = b.getFValue(div.margin.left(), parentFreeSize);
                }
            }

            var parentHeight = b.getParentFreeHeight(div.parent);
            var elHeight = div.element.sizeDelta.y + div.margin.vertical();

            if (parentHeight >= elHeight)
            {
                if (div.margin.top().auto && div.margin.bottom().auto)
                {
                    var remainingSpace = parentHeight - elHeight;
                    div.margin.top().floating = remainingSpace / 2;
                }
                else
                {
                    div.margin.top().floating = b.getFValue(div.margin.top(), parentHeight);
                }
            }

            float x = div.margin.left().floating;
            float y = div.margin.top().floating;

            x = x + div.parent.padding.left().floating;

            if (div.childIndex > 0)
            {
                float siblingY;
                float biggestSiblingHeight;
                var ocupiedSpace = getOcupiedSpace(div, out siblingY, out biggestSiblingHeight);

                siblingY = Mathf.Abs(siblingY);

                // check if this div fits in with the rest.
                //var pS = (int)parentFreeSize;
                if (parentFreeSize < (ocupiedSpace + elSize))   // div doesn't fit move down + the height of its biggest sibling.
                {
                    y = siblingY + y + biggestSiblingHeight;
                }
                else
                {
                    if (siblingY == 0)
                        y = y + div.parent.padding.top().floating;

                    x = ocupiedSpace + x;
                    y = siblingY + y;
                }
            }
            else
            {
                y = y + div.parent.padding.top().floating;
            }


            div.element.localPosition = new Vector3(x, -y, 0f);
        }

        private static float getOcupiedSpace(div div, out float siblingY, out float biggestSiblingHeight)
        {
            var siblingIndex = div.childIndex - 1;
            var sibling = div.parent.children;

            siblingY = sibling[siblingIndex].element.localPosition.y;

            biggestSiblingHeight = sibling[siblingIndex].element.sizeDelta.y + sibling[siblingIndex].margin.vertical();

            var space = 0f;
            space += sibling[siblingIndex].element.sizeDelta.x + sibling[siblingIndex].margin.horizontal();

            if (siblingIndex != 0)
            {
                for (var i = siblingIndex - 1; i >= 0; i--)
                {
                    if ((int)sibling[i].element.localPosition.y != (int)siblingY)
                        continue;

                    if (biggestSiblingHeight <
                        sibling[i].element.sizeDelta.y + sibling[siblingIndex].margin.vertical())
                        biggestSiblingHeight = sibling[i].element.sizeDelta.y +
                                               sibling[siblingIndex].margin.vertical();

                    space += sibling[i].element.sizeDelta.x + sibling[i].margin.horizontal();
                }
            }

            // getting it without the margin
            if (siblingY < 0)
                siblingY = siblingY + sibling[siblingIndex].margin.top().floating;
            else
                siblingY = siblingY - sibling[siblingIndex].margin.top().floating;

            return space;
        }
    }

    public enum AnchorType
    {
        TopRight, TopLeft,
        LeftCenter,
        Center
    }

    public enum LayoutType
    {
        Square,
        Full
    }

    public enum AspectRatio
    {
        Unregistered,
        Aspect_16_9
    }

    public enum bootstrap
    {
        boot,
        px,
        percent,
        none
    }

    public enum divType
    {
        div,
        label,
        body,
        button
    }

    public enum _class
    {
        none,
        round_header
    }

    public enum ElementType
    {
        None,
        InputField,
        NavbarButton,
        ScrollViewButton,
        TextField
    }

    public static class style_class
    {
        public static void buildClass(div div)
        {
            switch (div._class)
            {
                case _class.round_header:
                    round_header(div);
                    break;
            }
        }

        public static void round_header(div div)
        {
            RectTransform longPart = null;
            RectTransform emptyPart = null;
            RectTransform smallPart = null;

            foreach (Transform child in div.element.transform)
            {
                switch (child.gameObject.name)
                {
                    case "long":
                        longPart = child.GetComponent<RectTransform>();
                        break;
                    case "empty":
                        emptyPart = child.GetComponent<RectTransform>();
                        break;
                    case "small":
                        smallPart = child.GetComponent<RectTransform>();
                        break;
                }
            }

            Debug.Assert(longPart != null, "longPart != null");
            Debug.Assert(emptyPart != null, "emptyPart != null");
            Debug.Assert(smallPart != null, "smallPart != null");

            var height = div.element.sizeDelta.y;
            var emptyPartWidth = height * 2;
            var smallPartWidth = height;

            var width = div.element.sizeDelta.x;

            var remainingWidth = width - emptyPartWidth - smallPartWidth;

            //style_utils.GetValuePercent(emptyPartWidth, width);

            longPart.sizeDelta = new Vector3(remainingWidth, height);
            longPart.localPosition = new Vector3(0, 0, 0);

            emptyPart.sizeDelta = new Vector3(emptyPartWidth, height);
            emptyPart.localPosition = new Vector3(remainingWidth, 0, 0);

            smallPart.sizeDelta = new Vector3(smallPartWidth, height);
            smallPart.localPosition = new Vector3(remainingWidth + emptyPartWidth, 0, 0);
        }

        public static ElementType GetElementType(string name)
        {
            ElementType elType = ElementType.InputField;
            if (name.Contains("InputField"))
            {
                elType = ElementType.InputField;
            }
            else if (name.Contains("NavbarButton"))
            {
                elType = ElementType.NavbarButton;
            }
            else if (name.Contains("ScrollViewButton"))
            {
                elType = ElementType.ScrollViewButton;
            }
            return elType;
        }
    }
    
    public static class style_base
    {
        public static int ScreenWidth;
        public static int ScreenHeight;

        public static readonly Color32 Transparent = new Color32(0, 0, 0, 0);
        public static readonly Color32 Black = new Color32(0, 0, 0, 255);
        public static readonly Color32 White = new Color32(255, 255, 255, 255);
        public static readonly Color32 WhiteTransparent = new Color32(255, 255, 255, 200);

        public static readonly string circle = "";

        public static value getValue(value value)
        {
            if (value == null)
                value = new value { natural = 12, boot = bootstrap.boot };

            if (value.boot == bootstrap.boot)
                value.floating = ComputeSize(value.natural);

            return value;
        }

        public static value getValue(value value, float size)
        {
            if (value == null)
                value = new value { natural = 12, boot = bootstrap.boot };

            if (value.boot == bootstrap.boot)
            {
                value.floating = ComputeSize(value.natural);
                value.floating = GetPercent(size, value.floating);
            }
            else if (value.boot == bootstrap.percent)
                value.floating = GetPercent(size, value.natural);

            return value;
        }

        public static float getFValue(value value, float size)
        {
            if (value == null)
                value = new value { natural = 12, boot = bootstrap.boot };

            if (value.boot == bootstrap.px)
                return value.floating;

            if (value.boot == bootstrap.boot)
            {
                value.floating = ComputeSize(value.natural);
                return GetPercent(size, value.floating);
            }
            if (value.boot == bootstrap.percent)
                return GetPercent(size, value.natural);
            return 0f;
        }

        public static float ComputeSize(float size)
        {
            float val = 100.0f;
            switch ((int)size)
            {
                case 12:
                    val = 100.0f;
                    break;
                case 11:
                    val = 91.66f;
                    break;
                case 10:
                    val = 83.33f;
                    break;
                case 9:
                    val = 75.0f;
                    break;
                case 8:
                    val = 66.66f;
                    break;
                case 7:
                    val = 58.33f;
                    break;
                case 6:
                    val = 50.0f;
                    break;
                case 5:
                    val = 41.66f;
                    break;
                case 4:
                    val = 33.33f;
                    break;
                case 3:
                    val = 25.0f;
                    break;
                case 2:
                    val = 16.66f;
                    break;
                case 1:
                    val = 8.33f;
                    break;
            }
            return val;
        }

        public static bool isBody(string s)
        {
            if (s.Contains("<body"))
                return true;

            return false;
        }

        public static bool isDiv(Transform t)
        {
            if (t.gameObject.name[0] != '<')
                return false;
            return true;
        }

        public static bool isLabel(string s)
        {
            if (s.Contains("Label"))
                return true;

            return false;
        }

        public static bool isButton(string s)
        {
            if (s.Contains("Button"))
                return true;

            return false;
        }

        public static float getParentFreeWidth(div p)
        {
            return p.element.sizeDelta.x - p.padding.horizontal();
        }

        public static float getParentFreeHeight(div p)
        {
            return p.element.sizeDelta.y - p.padding.vertical();
        }

        public static float GetPercent(float value, float percent)
        {
            return (value / 100f) * percent;
        }

        public static float GetValuePercent(float value, float maxValue)
        {
            return (value * 100f) / maxValue;
        }

        public static void SetSize(float x, float y, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            var w = style_utils.GetPercent(parent.sizeDelta.x, x);
            var h = style_utils.GetPercent(parent.sizeDelta.y, y);

            rt.GetComponent<RectTransform>().sizeDelta = new Vector3(w, h);
        }

        public static void SetSize(float x, float y, Button button)
        {
            SetSize(x, y, button.GetComponent<RectTransform>());
            for (int i = 0; i < button.transform.childCount; i++)
            {
                var text = button.transform.GetChild(i).GetComponent<Text>();
                if (text != null)
                    SetTextSize(text, 45);
            }
        }

        public static void SetSize(LayoutType layoutType, RectTransform rt)
        {
            if (layoutType == LayoutType.Full)
            {
                rt.sizeDelta = new Vector3(ScreenWidth, ScreenHeight);
            }
        }

        public static void SetSize(float x, LayoutType layoutType, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            var w = style_utils.GetPercent(parent.sizeDelta.x, x);
            float h = w;

            rt.sizeDelta = new Vector3(w, h);
        }

        public static void SetSize(LayoutType layoutType, float y, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            float w;
            var h = style_utils.GetPercent(parent.sizeDelta.y, y);
            w = h;

            rt.sizeDelta = new Vector3(w, h);
        }

        public static void SetFixedSize(float x, float y, RectTransform rt)
        {
            rt.GetComponent<RectTransform>().sizeDelta = new Vector3(x, y);
        }

        public static void SetPosition(float x, float y, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            var w = style_utils.GetPercent(parent.sizeDelta.x, x);
            var h = style_utils.GetPercent(parent.sizeDelta.y, y);

            rt.GetComponent<RectTransform>().localPosition = new Vector3(w, -h, 0f);
        }

        public static void SetFixedPosition(float x, float y, RectTransform rt)
        {
            rt.GetComponent<RectTransform>().localPosition = new Vector3(x, -y, 0f);
        }

        public static void SetAnchor(AnchorType at, RectTransform rt)
        {
            Vector2 anchor;
            Vector2 pivot;

            if (at == AnchorType.TopLeft)
            {
                anchor = new Vector2(0, 1);
                pivot = new Vector2(0, 1);
            }
            else if (at == AnchorType.LeftCenter)
            {
                anchor = new Vector2(0, 1f);
                pivot = new Vector2(0f, 0.5f);
            }
            else if (at == AnchorType.Center)
            {
                anchor = new Vector2(0.5f, 0.5f);
                pivot = new Vector2(0.5f, 0.5f);
            }
            else
            {
                anchor = new Vector2(1, 1);
                pivot = new Vector2(1, 1);
            }

            rt.anchorMin = anchor;
            rt.anchorMax = anchor;
            rt.pivot = pivot;
        }

        public static void SetIconSize(Text text)
        {
            var parent = text.transform.parent.GetComponent<RectTransform>();

            text.fontSize = (int)parent.sizeDelta.y;
        }

        public static void SetTextSize(Text text, float predefinedValue = 0)
        {
            var parentHeight = text.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
            var textHeight = text.transform.GetComponent<RectTransform>().sizeDelta.y;

            if (predefinedValue > 1)
            {
                text.fontSize = (int)style_utils.GetPercent(parentHeight, predefinedValue);
                return;
            }
            else if (Math.Abs(textHeight) < 1)
            {
                text.fontSize = (int)style_utils.GetPercent(parentHeight, 99);
                return;
            }
            var ratio = style_utils.GetValuePercent(textHeight, parentHeight);
            text.fontSize = (int)style_utils.GetPercent(textHeight, ratio);
        }

        public static float GCD(float x, float y)
        {
            return Math.Abs(y) < 1 ? x : GCD(y, x % y);
        }

        public static AspectRatio GetAspectRatio()
        {
            var x = ScreenWidth / GCD(ScreenWidth, ScreenHeight);
            var y = ScreenHeight / GCD(ScreenWidth, ScreenHeight);

            if ((Math.Abs(x - 16) < 0.1f && Math.Abs(y - 9) < 0.1f) ||
                (Math.Abs(x - 427) < 2 && Math.Abs(y - 240) < 2) ||
                (Math.Abs(x - 80) < 2 && Math.Abs(y - 39) < 2) ||  //  480	× 234
                (Math.Abs(x - 30) < 2 && Math.Abs(y - 17) < 2) ||  //  480	×	272
                (Math.Abs(x - 53) < 2 && Math.Abs(y - 30) < 2) ||
                (Math.Abs(x - 128) < 2 && Math.Abs(y - 75) < 2) ||
                (Math.Abs(x - 71) < 2 && Math.Abs(y - 40) < 2) ||
                (Math.Abs(x - 667) < 2 && Math.Abs(y - 375) < 2) ||
                (Math.Abs(x - 683) < 2 && Math.Abs(y - 384) < 2) ||
                (Math.Abs(x - 222) < 2 && Math.Abs(y - 125) < 2))
            {
                return AspectRatio.Aspect_16_9;
            }

            return AspectRatio.Unregistered;
        }
    }
}