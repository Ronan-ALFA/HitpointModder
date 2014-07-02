using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Drawing;
/*
 * NWN2Toolset.dll exposes all the functions we need to manupulate the toolkit.
 */
using NWN2Toolset.Plugins;
/*
 * Sandbar is the library thats used for the toolbar.
 * 
 * Windows also has a Toolbar object in System.Windows.Forms so make sure
 * you create the correct object when adding a toolbar to the toolkit.
 */
using TD.SandBar;

namespace HitpointsModder
{
    public class HitpointMaxer : INWN2Plugin
    {

        private MenuButtonItem m_cMenuItem;

        private void HandlePluginLaunch(object sender, EventArgs e)
        {
            var creatures = NWN2Toolset.NWN2.Data.Blueprints.NWN2GlobalBlueprintManager.GetBlueprintsOfType(NWN2Toolset.NWN2.Data.Templates.NWN2ObjectType.Creature);

            foreach (NWN2Toolset.NWN2.Data.Blueprints.NWN2CreatureBlueprint creature in creatures) {
                var core = creature.GetStatsCore();
                short bhp, mhp;
                if (creature.Race.Row == 24)
                {
                    mhp = bhp = (short)(core.GetLevel() * 12);
                }
                else
                {
                    bhp = (short)core.CalcMaxHitpointsFromHitDice();
                    mhp = (short)(bhp + core.CalcHitPointModsFromFeats(1) + core.CalcHitPointModsFromStats());
                }                
                creature.BaseHitPoints = bhp;
                creature.CharsheetHitPoints = mhp;
                creature.CurrentHitPoints = mhp;
            }
        }

        public void Load(INWN2PluginHost cHost)
        {
        }

        public void Shutdown(INWN2PluginHost cHost)
        {
        }

        public void Startup(INWN2PluginHost cHost)
        {
            m_cMenuItem = cHost.GetMenuForPlugin(this);
            m_cMenuItem.Activate += new EventHandler(this.HandlePluginLaunch);
        }

        public void Unload(INWN2PluginHost cHost)
        {
        }

        public MenuButtonItem PluginMenuItem
        {
            get
            {
                return m_cMenuItem;
            }
        }

        // Properties
        public string DisplayName
        {
            get
            {
                return "Hitpoint Maxer";
            }
        }

        public string MenuName
        {
            get
            {
                return "Hitpoint Maxer";
            }
        }

        public string Name
        {
            get
            {
                return "Hitpoint Maxer";
            }
        }


        public object Preferences
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
    }
}
