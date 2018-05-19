using System;
using System.IO;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public class TabPanel : IDisposable
    {
        private readonly TextWriter textWriter;
        private readonly IExtensoUIProvider provider;

        public string Id { get; private set; }

        public bool IsActive { get; private set; }

        internal TabPanel(IExtensoUIProvider provider, TextWriter writer, string id, bool isActive = false)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.provider = provider;
            Id = id;
            IsActive = isActive;
            textWriter = writer;
            provider.TabsProvider.BeginTabPanel(this, textWriter);
        }

        public void Dispose()
        {
            provider.TabsProvider.EndTabPanel(textWriter);
        }
    }
}