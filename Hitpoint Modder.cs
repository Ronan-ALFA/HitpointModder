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

        enum HpType { Max, Half, Average }
        private MenuButtonItem m_cMenuItem;

        private void MaxHp(object sender, EventArgs e) {
            ModHp(HpType.Max);
        }

        private void HalfHp(object sender, EventArgs e)
        {
            ModHp(HpType.Half);
        }

        private void AverageHp(object sender, EventArgs e)
        {
            ModHp(HpType.Average);
        }

        private void ModHp(HpType type)
        {
            var creatures = NWN2Toolset.NWN2.Data.Blueprints.NWN2GlobalBlueprintManager.GetBlueprintsOfType(NWN2Toolset.NWN2.Data.Templates.NWN2ObjectType.Creature);

            foreach (NWN2Toolset.NWN2.Data.Blueprints.NWN2CreatureBlueprint creature in creatures)
            {
                var core = creature.GetStatsCore();
                int baseHp, hpBonus;
                int hd = core.GetLevel();
                int featHpBonus = core.CalcHitPointModsFromFeats(1);
                if (creature.Race.Row == 24)
                {
                    baseHp = hd * 12;
                    hpBonus = featHpBonus;
                }
                else
                {
                    baseHp = core.CalcMaxHitpointsFromHitDice();
                    hpBonus = (featHpBonus + core.CalcHitPointModsFromStats());
                }
                if (type == HpType.Half)
                {
                    baseHp = (baseHp / 2);

                }
                else if (type == HpType.Average)
                {
                    baseHp = (int)((baseHp / 2) + hd * 0.5);
                }

                var totalHp = baseHp + hpBonus;
                creature.BaseHitPoints = (short)baseHp;
                creature.CharsheetHitPoints = (short)totalHp;
                creature.CurrentHitPoints = (short)totalHp;
            }
        }

        public void Load(INWN2PluginHost cHost) {}

        public void Shutdown(INWN2PluginHost cHost) {}

        public void Startup(INWN2PluginHost cHost)
        {
            m_cMenuItem = cHost.GetMenuForPlugin(this);
            m_cMenuItem.Items.Add("Maximize all creature HP", new EventHandler(this.MaxHp));
            m_cMenuItem.Items.Add("Halve all creature HP", new EventHandler(this.HalfHp));
            m_cMenuItem.Items.Add("Average all creature HP", new EventHandler(this.AverageHp));
        }

        public void Unload(INWN2PluginHost cHost) {}

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
                return "Hitpoint Modder";
            }
        }

        public string MenuName
        {
            get
            {
                return "Hitpoint Modder";
            }
        }

        public string Name
        {
            get
            {
                return "Hitpoint Modder";
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
