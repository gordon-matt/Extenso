﻿using System;
using System.IO;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public class Panel : HtmlElement
    {
        public string Id { get; private set; }

        public State State { get; private set; }

        public Panel(string id = null, State state = State.Primary, object htmlAttributes = null)
            : base(htmlAttributes)
        {
            if (id == null)
            {
                id = $"panel-{Guid.NewGuid()}";
            }
            Id = id;
            State = state;
            EnsureHtmlAttribute("id", Id);
        }

        protected internal override void StartTag(TextWriter textWriter)
        {
            Provider.PanelProvider.BeginPanel(this, textWriter);
        }

        protected internal override void EndTag(TextWriter textWriter)
        {
            Provider.PanelProvider.EndPanel(this, textWriter);
        }
    }
}