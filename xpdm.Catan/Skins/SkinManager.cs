using System;
using C5;
using SCG=System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Windows.Markup;
using System.Windows.Media;
using System.IO;

namespace xpdm.Catan.Skins
{
    public class SkinManager
    {
        private const string DefaultSkinName = "Default";
        private static readonly SkinManager s_instance = new SkinManager();

        public static SkinManager Instance
        {
            get { return s_instance; }
        }

        private readonly IList<SkinDescription> _skinDescriptions;
        private readonly IList<SkinDescription> _guardedSkinDescriptions;

        public IList<SkinDescription> Skins
        {
            get { return _guardedSkinDescriptions; }
        }

        private SkinManager()
        {
            _skinDescriptions = new ArrayList<SkinDescription>();
            _guardedSkinDescriptions = new GuardedList<SkinDescription>(_skinDescriptions);

            FindAvailableSkins();
        }

        private SkinDescription LoadSkinDescription(string skinName)
        {
            SkinDescription skinDescription = null;
            var skinPath = "Skins/" + skinName + "/Description.xaml";
            try
            {
                skinDescription = Application.LoadComponent(new Uri("/xpdm.Catan;component/" + skinPath, UriKind.Relative)) as SkinDescription;
            }
            catch (IOException)
            {
                /* Handle IOException by assuming component doesn't exist. Swallow error and trace.*/
                Trace.TraceInformation("Skin description '{0}' not a component.", skinName);
            }
            if (skinDescription != null)
            {
                skinDescription.Name = skinName;
                return skinDescription;
            }

            if (File.Exists(skinPath))
            {
                using (FileStream s = File.OpenRead(skinPath))
                {
                    skinDescription = (SkinDescription)XamlReader.Load(s, new ParserContext { BaseUri = new Uri("pack://application:,,,/" + skinPath, UriKind.Absolute) });
                }
            }
            if (skinDescription == null)
            {
                return new SkinDescription
                {
                    Name = skinName,
                    DisplayName = skinName,
                    Preview = (Visual)Application.Current.TryFindResource("DefaultSkinPreview"),
                };
            }
            skinDescription.Name = skinName;
            return skinDescription;
        }

        private void FindAvailableSkins()
        {
            _skinDescriptions.Clear();
            _skinDescriptions.Add(LoadSkinDescription(DefaultSkinName));
            try
            {
                if (Directory.Exists("Skins"))
                {
                    var dirs = from dir in Directory.EnumerateDirectories("Skins")
                               where File.Exists(Path.Combine(dir, "Style.xaml"))
                               select Path.GetFileName(dir);
                    foreach (var dir in dirs)
                    {
                        _skinDescriptions.Add(LoadSkinDescription(dir));
                    }
                }
            }
            catch (IOException)
            { }
        }

        public void ApplySkin(SkinDescription skin)
        {
            var oldSkinDefinition = Application.Current.Properties["CurrentSkin"] as ResourceDictionary;
            if (oldSkinDefinition != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(oldSkinDefinition);
            }
            if (skin.Name != DefaultSkinName)
            {
                ResourceDictionary newSkinDefinition = null;
                var newSkinPath = "Skins/" + skin.Name + "/Style.xaml";
                using (FileStream s = File.OpenRead(newSkinPath))
                {
                    newSkinDefinition = (ResourceDictionary)XamlReader.Load(
                        s,
                        new ParserContext 
                        { 
                            BaseUri = new Uri("pack://application:,,,/" + newSkinPath, 
                                UriKind.Absolute), 
                        });
                }
                if (newSkinDefinition != null)
                {
                    Application.Current.Resources.MergedDictionaries.Add(newSkinDefinition);
                    Application.Current.Properties["CurrentSkin"] = newSkinDefinition;
                }
            }
            Application.Current.Properties["CurrentSkinDescription"] = LoadSkinDescription(skin.Name) ?? LoadSkinDescription("Default");

        }
    }
}
