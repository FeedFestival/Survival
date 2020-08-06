using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Utils;
using b = Assets.Scripts.Utils.style_base;

public class style : MonoBehaviour
{
    public element _element;

    public element element
    {
        get
        {
            if (_element == null)
                _element = new element(transform);
            return _element;
        }
    }

    private div _div;
    public div Div
    {
        get
        {
            if (_div == null)
                _div = new div
                {
                    Id = Name,
                    elementName = gameObject.name,
                    type = divType.div,
                    element = GetComponent<RectTransform>(),
                    children = new List<div>()
                };
            return _div;
        }
    }

    [SerializeField]
    public ElementType ElementClass;

    [HideInInspector]
    public string Name;

    [HideInInspector]
    public string Class;

    public value Height
    {
        get
        {
            return Div.height;
        }
    }

    public value[] Margin;

    public void Calculate()
    {
        var pTransform = transform.parent.transform;
        var _parent = new div
        {
            Id = 0.ToString(),
            elementName = pTransform.gameObject.name,
            children = new List<div>(),
            type = pTransform.gameObject.name.Contains("<body") ? divType.body : divType.div,
            element = pTransform.GetComponent<RectTransform>()
        };
        style_utils.getDivs(pTransform, _parent);
        style_utils.buildDiv(_div);
    }

    public void Refresh()
    {
        _div = new div
        {
            Id = Name,
            elementName = gameObject.name,
            type = divType.div,
            element = GetComponent<RectTransform>(),
            children = new List<div>()
        };
    }

    void Start()
    {
        if (ElementClass == ElementType.None)
            return;
        _element = new element(transform);
    }
}

namespace Assets.Scripts.Utils
{
    public class element
    {
        public ElementType ElementType;

        public Text Placeholder;
        public Text Text;

        public GameObject InnerShadow;

        public GameObject shadow;

        public element(Transform transform, ElementType el = ElementType.None)
        {
            ElementType = style_class.GetElementType(transform.gameObject.name);

            switch (ElementType)
            {
                case ElementType.InputField:

                    foreach (Transform child in transform)
                    {
                        if (child.gameObject.name == "Placeholder")
                        {
                            Placeholder = child.GetComponent<Text>();
                        }
                        else if (child.gameObject.name == "Text")
                        {
                            Text = child.GetComponent<Text>();
                        }
                    }

                    break;

                case ElementType.NavbarButton:

                    foreach (Transform child in transform)
                    {
                        if (child.gameObject.name == "inner_shadow")
                        {
                            InnerShadow = child.gameObject;
                        }
                        else if (child.gameObject.name == "Text")
                        {
                            Text = child.GetComponent<Text>();
                        }
                    }
                    break;

                case ElementType.ScrollViewButton:

                    foreach (Transform child in transform)
                    {
                        if (child.gameObject.name == "shadow")
                        {
                            shadow = child.gameObject;
                        }
                        else if (child.gameObject.name == "Text")
                        {
                            Text = child.GetComponent<Text>();
                        }
                    }
                    break;

                case ElementType.TextField:


                    foreach (Transform child in transform)
                    {
                        if (child.gameObject.name == "Text")
                        {
                            Text = child.GetComponent<Text>();
                        }
                    }
                    break;
            }
        }

        public void SetActive(bool val)
        {
            switch (ElementType)
            {
                case ElementType.None:
                    break;
                case ElementType.InputField:
                    break;
                case ElementType.NavbarButton:
                    if (InnerShadow != null)
                        InnerShadow.SetActive(val);

                    if (val)
                    {
                        Text.CrossFadeColor(utils.black, 0.5f, true, false);
                    }
                    else
                    {
                        Text.CrossFadeColor(utils.white, 0.5f, false, false);
                    }
                    break;
                case ElementType.ScrollViewButton:
                    if (shadow != null)
                        shadow.SetActive(val);

                    if (val)
                    {
                        Text.CrossFadeColor(utils.gold, 0.5f, true, false);
                    }
                    else
                    {
                        Text.CrossFadeColor(utils.grey, 0.5f, false, false);
                    }
                    break;

                case ElementType.TextField:


                    break;

                //default:
                //    throw new ArgumentOutOfRangeException("elementType", ElementType, null);
            }
        }
    }

    public class multiple
    {
        private value[] values;

        public multiple()
        {
            CreateEmpty();
        }

        private void CreateEmpty()
        {
            values = new value[4];
            var val = new value
            {
                floating = 0f,
                boot = bootstrap.px
            };
            left(val);
            top(val);
            right(val);
            bottom(val);
        }

        public value left()
        {
            return values[0];
        }

        public void left(value val)
        {
            if (values == null)
                CreateEmpty();
            values[0] = val;
        }

        public value top()
        {
            return values[1];
        }

        public void top(value val)
        {
            if (values == null)
                CreateEmpty();
            values[1] = val;
        }

        public value right()
        {
            return values[2];
        }

        public void right(value val)
        {
            if (values == null)
                CreateEmpty();
            values[2] = val;
        }

        public value bottom()
        {
            return values[3];
        }

        public void bottom(value val)
        {
            if (values == null)
                CreateEmpty();
            values[3] = val;
        }

        public float horizontal()
        {
            if (values == null)
                CreateEmpty();
            return left().floating + right().floating;
        }

        public float vertical()
        {
            if (values == null)
                CreateEmpty();
            return top().floating + bottom().floating;
        }

        public override string ToString()
        {
            return string.Format("-[{0}, {1}, {2}, {3}]", values[0].floating, values[1].floating, values[2].floating, values[3].floating);
        }
    }

    public class div
    {
        public string Id;
        public string elementName;
        public divType type;
        public int childIndex;

        public bool ignoreChildren;

        public div parent;
        //public List<boot> children;

        public List<div> children;

        public value width;
        public value height;

        // 0 = left, 1 = top, 2 = right, 3 = bottom

        public multiple margin;

        public multiple padding;

        public _class _class;

        public bool sub_class;

        public override string ToString()
        {
            return elementName;
        }

        private RectTransform _element;
        public RectTransform border;

        public RectTransform element
        {
            get { return _element; }
            set
            {
                _element = value;

                if (type != divType.body)
                {
                    // size
                    if (elementName.Contains("("))
                    {
                        var s = elementName.Split('(', ')');
                        var values = s[1].Split(',');

                        formatSize(values[0], out width);

                        if (values.Length == 1)
                            formatSize("p-100", out height);
                        else
                            formatSize(values[1], out height);
                    }
                }

                // does it have a class.
                if (elementName.Contains("-c("))
                {
                    var s = elementName.Split(new[] { "-c(", ")c" }, StringSplitOptions.None);
                    _class = (_class)Enum.Parse(typeof(_class), s[1], true);

                }

                // padding
                if (elementName.Contains("|"))
                {
                    var s = elementName.Split('|', '|');
                    var values = s[1].Split(',');

                    padding = setupValues(values);
                }
                else
                {
                    padding = new multiple();
                }

                // margin
                if (elementName.Contains("["))
                {
                    var s = elementName.Split('[', ']');
                    var values = s[1].Split(',');

                    margin = setupValues(values);
                }
                else
                {
                    margin = new multiple();
                }

                if (elementName.Contains("-ignc"))
                    ignoreChildren = true;

                if (style_base.isLabel(elementName)
                    //|| style_base.isButton(elementName)
                    )
                {
                    if (style_base.isLabel(elementName))
                        type = divType.label;

                    style_utils.SetAnchor(AnchorType.Center, _element);
                }
                else if (style_base.isButton(elementName))
                    type = divType.button;


                if (type == divType.label && margin == null)
                {
                    value val;
                    margin = new multiple();

                    formatSize("auto", out val);

                    margin.left(val);
                    margin.top(val);
                    margin.right(val);
                    margin.bottom(val);
                }
            }
        }

        private multiple setupValues(string[] values)
        {
            var multi = new multiple();
            value val;

            switch (values.Length)
            {
                case 1:
                    formatSize(values[0], out val);
                    multi.left(val);
                    multi.right(val);
                    multi.top(val);
                    multi.bottom(val);
                    break;
                case 2:
                    formatSize(values[0], out val);
                    multi.left(val);
                    multi.right(val);

                    formatSize(values[1], out val);
                    multi.top(val);
                    multi.bottom(val);
                    break;
                case 3:
                    formatSize(values[0], out val);
                    multi.left(val);

                    formatSize(values[1], out val);
                    multi.right(val);

                    formatSize(values[2], out val);
                    multi.top(val);
                    multi.bottom(val);
                    break;
                case 4:
                    formatSize(values[0], out val);
                    multi.left(val);

                    formatSize(values[1], out val);
                    multi.top(val);

                    formatSize(values[2], out val);
                    multi.right(val);

                    formatSize(values[3], out val);
                    multi.bottom(val);
                    break;
            }
            return multi;
        }

        private void formatSize(string value, out value val)
        {
            val = new value();
            if (value.Equals("auto"))
            {
                val.auto = true;
            }
            else if (value.Contains("b-"))
            {
                value = value.Replace("b-", "");
                val.boot = bootstrap.boot;
                int.TryParse(value, out val.natural);
            }
            else if (value.Contains("p-"))
            {
                value = value.Replace("p-", "");
                val.boot = bootstrap.percent;
                int.TryParse(value, out val.natural);
            }
            else
            {
                val.boot = bootstrap.px;
                float.TryParse(value, out val.floating);
            }
        }
    }

    public class value
    {
        public int natural;
        public float floating;
        public bootstrap boot;
        public bool auto;
    }
}