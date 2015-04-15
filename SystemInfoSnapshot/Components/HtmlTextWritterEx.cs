/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace SystemInfoSnapshot.Components
{
    public class HtmlTextWriterEx : HtmlTextWriter
    {
        private Dictionary<HtmlTextWriterAttribute, List<string>> _attrValues = new Dictionary<HtmlTextWriterAttribute, List<string>>();
        private readonly HtmlTextWriterAttribute[] _allowedMultiValueAttrs = { HtmlTextWriterAttribute.Class, HtmlTextWriterAttribute.Style };

        public HtmlTextWriterEx(TextWriter writer) : base(writer) { }

        public override void AddAttribute(HtmlTextWriterAttribute key, string value)
        {
            if (_allowedMultiValueAttrs.Contains(key))
            {
                if (!_attrValues.ContainsKey(key))
                    _attrValues.Add(key, new List<string>());

                _attrValues[key].Add(value);
            }
            else
            {
                base.AddAttribute(key, value);
            }
        }

        public bool RemoveAttribute(HtmlTextWriterAttribute key, string value)
        {
            return _allowedMultiValueAttrs.Contains(key) && _attrValues.Remove(key);
        }

        public override void RenderBeginTag(HtmlTextWriterTag tagKey)
        {
            AddMultiValuesAttrs();
            base.RenderBeginTag(tagKey);
        }

        public override void RenderBeginTag(string tagName)
        {
            AddMultiValuesAttrs();
            base.RenderBeginTag(tagName);
        }

        public void RenderTag(HtmlTextWriterTag tagKey, string html)
        {
            RenderBeginTag(tagKey);
            if (!string.IsNullOrEmpty(html))
                Write(html);
            RenderEndTag();
        }

        public void RenderTag(string tagKey, string html)
        {
            RenderBeginTag(tagKey);
            Write(html);
            RenderEndTag();
        }

        public void RenderTag(HtmlTextWriterTag tagKey, HtmlTextWriterAttribute attribute, string attributeVal, string html)
        {
            AddAttribute(attribute, attributeVal);
            RenderTag(tagKey, html);
        }

       public void RenderTag(HtmlTextWriterTag tagKey, string attribute, string attributeVal, string html)
        {
            AddAttribute(attribute, attributeVal);
            RenderTag(tagKey, html);
        }


        private void AddMultiValuesAttrs()
        {
            foreach (var key in _attrValues.Keys)
                AddAttribute(key.ToString(), string.Join(" ", _attrValues[key].ToArray()));

            _attrValues = new Dictionary<HtmlTextWriterAttribute, List<string>>();
        }
    }
}
