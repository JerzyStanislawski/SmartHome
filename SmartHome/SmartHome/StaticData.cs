using SmartHome.Model;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome
{
    //TODO: To appconfig.json
    public class StaticData
    {
        public static Dictionary<int, Light> GroundLights { get; private set; } = new Dictionary<int, Light>();
        public static Dictionary<int, Light> AtticLights { get; private set; } = new Dictionary<int, Light>();
        public static Dictionary<int, Blind> GroundBlinds { get; private set; } = new Dictionary<int, Blind>();
        public static Dictionary<int, Blind> AtticBlinds { get; private set; } = new Dictionary<int, Blind>();
        public static Dictionary<string, string> LightNames { get; private set; }
        public static Dictionary<string, string> BlindNames { get; private set; }

        static StaticData()
        {
            InitializeGroundLights();
            InitializeAtticLights();
            InitializeGroundBlinds();
            InitializeAtticBlinds();
            InitializeLightsMap();
            InitializeBlindsMap();
        }

        private static void InitializeAtticBlinds()
        {
            AtticBlinds[Resource.Id.buttonAdam1Up] = new Blind("pokoj1_1_up", "Adam lewe", new[] { "u Adama lewą", "lewą u Adama" });
            AtticBlinds[Resource.Id.buttonAdam1Down] = new Blind("pokoj1_1_down", "Adam lewe", new[] { "u Adama lewą", "lewą u Adama" });
            AtticBlinds[Resource.Id.buttonAdam2Up] = new Blind("pokoj1_2_up", "Adam prawe", new[] { "u Adama prawą", "prawą u Adama" });
            AtticBlinds[Resource.Id.buttonAdam2Down] = new Blind("pokoj1_2_down", "Adam prawe", new[] { "u Adama prawą", "prawą u Adama" });
            AtticBlinds[Resource.Id.buttonUlaUp] = new Blind("pokoj2_up", "Ula", new[] { "u Uli" });
            AtticBlinds[Resource.Id.buttonUlaDown] = new Blind("pokoj2_down", "Ula", new[] { "u Uli" });
            AtticBlinds[Resource.Id.buttonSypialniaUp] = new Blind("sypialnia_up", "Sypialnia", new[] { "w sypialni" });
            AtticBlinds[Resource.Id.buttonSypialniaDown] = new Blind("sypialnia_down", "Sypialnia", new[] { "w sypialni" });
        }

        private static void InitializeGroundBlinds()
        {
            GroundBlinds[Resource.Id.buttonSalon1Up] = new Blind("salon1_up", "Salon - prawa", new[] { "w salonie prawą", "prawą w salonie" });
            GroundBlinds[Resource.Id.buttonSalon1Down] = new Blind("salon1_down", "Salon - prawa", new[] { "w salonie prawą", "prawą w salonie" });
            GroundBlinds[Resource.Id.buttonSalon2Up] = new Blind("salon2_up", "Salon - lewa", new[] { "w salonie lewą", "lewą w salonie" });
            GroundBlinds[Resource.Id.buttonSalon2Down] = new Blind("salon2_down", "Salon - lewa", new[] { "w salonie lewą", "lewą w salonie" });
            GroundBlinds[Resource.Id.buttonJadalniaUp] = new Blind("jadalnia_up", "Jadalnia", new [] {"w jadalni"});
            GroundBlinds[Resource.Id.buttonJadalniaDown] = new Blind("jadalnia_down", "Jadalnia", new[] { "w jadalni" });
            GroundBlinds[Resource.Id.buttonKotlowniaUp] = new Blind("kotlownia_up", "Kotłownia", new[] { "w kotłowni" });
            GroundBlinds[Resource.Id.buttonKotlowniaDown] = new Blind("kotlownia_down", "Kotłownia", new[] { "w kotłowni" });
            GroundBlinds[Resource.Id.buttonKuchniaUp] = new Blind("kuchnia_up", "Kuchnia", new[] { "w kuchni" });
            GroundBlinds[Resource.Id.buttonKuchniaDown] = new Blind("kuchnia_down", "Kuchnia", new[] { "w kuchni" });
            GroundBlinds[Resource.Id.buttonGabinetUp] = new Blind("gabinet_up", "Gabinet", new[] { "w gabinecie" });
            GroundBlinds[Resource.Id.buttonGabinetDown] = new Blind("gabinet_down", "Gabinet", new[] { "w gabinecie" });
            GroundBlinds[Resource.Id.buttonWiatrolapUp] = new Blind("garderoba_up", "Wiatrołap", new[] { "w wiatrołapie" });
            GroundBlinds[Resource.Id.buttonWiatrolapDown] = new Blind("garderoba_down", "Wiatrołap", new[] { "w wiatrołapie" });
        }

        private static void InitializeAtticLights()
        {
            AtticLights[Resource.Id.switchSypialnia] = new Light("sypialnia", 19, "Sypialnia", new[] { "w sypialni" });
            AtticLights[Resource.Id.switchAdam] = new Light("pokoj1", 20, "Adam", new[] { "u Adama", "u Adasia" });
            AtticLights[Resource.Id.switchUla] = new Light("pokoj2", 21, "Ula", new[] { "u Uli", "u Ulki" });
            AtticLights[Resource.Id.switchLazienka] = new Light("lazienka_gora", 22, "Łazienka - główne", new[] { "w łazience na górze" });
            AtticLights[Resource.Id.switchLazienkaKinkiety] = new Light("lazienka_kinkiety", 23, "Łazienka - kinkiety", new[] { "w łazience kinkiety", "w łazience na górze kinkiety" });
            AtticLights[Resource.Id.switchLazienkaLed] = new Light("lazienka_led", 24, "Łazienka - LED", new[] { "w łazience led", "w łazience ledy", "w łazience na górze led", "w łazience na górze ledy" });
            AtticLights[Resource.Id.switchGarderoba] = new Light("garderoba_gora", 26, "Garderoba", new[] { "w garderobie" });
            AtticLights[Resource.Id.switchPralnia] = new Light("pralnia", 25, "Pralnia", new[] { "w pralni" });
            AtticLights[Resource.Id.switchKorytarz] = new Light("korytarz", 27, "Korytarz", new[] { "w korytarzu na górze", "na korytarzu na górze" });
            AtticLights[Resource.Id.switchKorytarzLed] = new Light("korytarz_nocne", 28, "Korytarz - nocne", new[] { "na korytarzu nocne", "w korytarzu nocne" });
            AtticLights[Resource.Id.switchSchody] = new Light("schody", 29, "Schody", new[] { "na schodach" });
            //AtticLights[Resource.Id.switchSchodyLed] = new Light("schody_led", OSW_SCHODY_LED, "Schody - nocne", new[] { "na schodach nocne" });
        }

        private static void InitializeGroundLights()
        {
            GroundLights[Resource.Id.switchSalon] = new Light("salon", 17, "Salon - główne", new[] { "w salonie", "w salonie duże", "w salonie główne" });
            GroundLights[Resource.Id.switchSalonKominek] = new Light("salon_kominek", 15, "Salon - kominek", new[] { "w salonie przy kominku", "w salonie kominek" });
            GroundLights[Resource.Id.switchSalonLed] = new Light("salon_led", 6, "Salon - LED", new[] { "w salonie led", "w salonie ledy" });
            GroundLights[Resource.Id.switchKuchnia] = new Light("kuchnia", 8, "Kuchnia - główne", new[] { "w kuchni", "w kuchni główne" });
            GroundLights[Resource.Id.switchSzafki] = new Light("kuchnia_szafki", 18, "Kuchnia - blat", new[] { "w kuchni nad blatem" });
            GroundLights[Resource.Id.switchSpizarnia] = new Light("spizarnia", 9, "Spiżarnia", new[] { "w spiżarni" });
            GroundLights[Resource.Id.switchJadalnia] = new Light("jadalnia", 7, "Jadalnia", new[] { "w jadalni" });
            GroundLights[Resource.Id.switchBarek] = new Light("barek", 0, "Barek", new[] { "nad barkiem" });
            GroundLights[Resource.Id.switchGabinet] = new Light("gabinet", 10, "Gabinet", new[] { "w gabinecie" });
            GroundLights[Resource.Id.switchLazienka] = new Light("lazienka", 11, "Łazienka - główne", new[] { "w łazience na dole" });
            GroundLights[Resource.Id.switchLustro] = new Light("lazienka_lustro", 16, "Łazienka - lustro", new[] { "w łazience na dole przy lustrze" });
            GroundLights[Resource.Id.switchHall] = new Light("hall", 4, "Hall", new[] { "w holu", "w korytarzu na dole", "na korytarzu na dole", "w przejściu na dole" });
            GroundLights[Resource.Id.switchWiatrolap] = new Light("wiatrolap", 3, "Wiatrołap", new[] { "w wiatrołapie" });
            GroundLights[Resource.Id.switchKotlownia] = new Light("kotlownia", 2, "Kotłownia", new[] { "w kotłowni" });
            GroundLights[Resource.Id.switchGaraz] = new Light("garaz", 1, "Garaż", new[] { "w garażu" });
            GroundLights[Resource.Id.switchWejscie] = new Light("wejscie", 12, "Wejście", new[] { "przy wejściu" });
        }

        private static void InitializeBlindsMap()
        {
            BlindNames = GroundBlinds.Concat(AtticBlinds).ToDictionary(x => x.Value.Name, x => x.Value.NiceName);
        }

        private static void InitializeLightsMap()
        {
            LightNames = GroundLights.Concat(AtticLights).ToDictionary(x => x.Value.Name, x => x.Value.NiceName);
        }
    }
}