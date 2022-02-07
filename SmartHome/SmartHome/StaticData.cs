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
            const int NUMER_DOMU = 22;
            const int OSW_KORYTARZ_NOCNE = 30;
            const int OSW_SYPIALNIA = 24;
            const int OSW_POKOJ_ADAM = 25;
            const int OSW_POKOJ_ULA = 26;
            const int OSW_SCHODY_LED = 27;
            const int ROL_GARDEROBA_DOWN = 28;
            const int OSW_SCHODY = 29;
            const int OSW_TARAS = 34;
            const int OSW_LAZIENKA_GORA = 31;
            const int OSW_TARAS_PUNKTOWE = 36;
            const int OSW_LAZIENKA_KINKIETY = 33;
            const int OSW_TARAS_KOLUMNA = 23;
            const int OSW_PRALNIA = 35;
            const int OSW_LAZIENKA_LED = 32;
            const int OSW_GARDEROBA_GORA = 37;
            const int OSW_KORYTARZ = 53;

            AtticLights[Resource.Id.switchSypialnia] = new Light("sypialnia", OSW_SYPIALNIA, "Sypialnia", new[] { "w sypialni" });
            AtticLights[Resource.Id.switchAdam] = new Light("pokoj1", OSW_POKOJ_ADAM, "Adam", new[] { "u Adama" });
            AtticLights[Resource.Id.switchUla] = new Light("pokoj2", OSW_POKOJ_ULA, "Ula", new[] { "u Uli" });
            AtticLights[Resource.Id.switchLazienka] = new Light("lazienka_gora", OSW_LAZIENKA_GORA, "Łazienka - główne", new[] { "w łazience na górze" });
            AtticLights[Resource.Id.switchLazienkaKinkiety] = new Light("lazienka_kinkiety", OSW_LAZIENKA_KINKIETY, "Łazienka - kinkiety", new[] { "w łazience kinkiety", "w łazience na górze kinkiety" });
            AtticLights[Resource.Id.switchLazienkaLed] = new Light("lazienka_led", OSW_LAZIENKA_LED, "Łazienka - LED", new[] { "w łazience led", "w łazience ledy", "w łazience na górze led", "w łazience na górze ledy" });
            AtticLights[Resource.Id.switchGarderoba] = new Light("garderoba_gora", OSW_GARDEROBA_GORA, "Garderoba", new[] { "w garderobie" });
            AtticLights[Resource.Id.switchPralnia] = new Light("pralnia", OSW_PRALNIA, "Pralnia", new[] { "w pralni" });
            AtticLights[Resource.Id.switchKorytarz] = new Light("korytarz", OSW_KORYTARZ, "Korytarz", new[] { "w korytarzu na górze", "na korytarzu na górze" });
            AtticLights[Resource.Id.switchKorytarzLed] = new Light("korytarz_nocne", OSW_KORYTARZ_NOCNE, "Korytarz - nocne", new[] { "na korytarzu nocne", "w korytarzu nocne" });
            AtticLights[Resource.Id.switchSchody] = new Light("schody", OSW_SCHODY, "Schody", new[] { "na schodach" });
            AtticLights[Resource.Id.switchSchodyLed] = new Light("schody_led", OSW_SCHODY_LED, "Schody - nocne", new[] { "na schodach nocne" });
        }

        private static void InitializeGroundLights()
        {
            const int OSW_GARAZ = 24;
            const int OSW_KOTLOWNIA = 25;
            const int OSW_WIATROLAP = 26;
            const int OSW_KORYTARZ = 28;
            const int OSW_SALON_LED = 30;
            const int OSW_JADALNIA = 31;
            const int OSW_KUCHNIA = 32;
            const int OSW_BAREK = 33;
            const int OSW_SPIZARNIA = 34;
            const int OSW_GABINET = 35;
            const int OSW_LAZIENKA = 36;
            const int OSW_WEJSCIE_GLOWNE = 37;
            const int OSW_WEJSCIE_KOLUMNA = 38;
            const int OSW_FRONT = 39;
            const int OSW_SALON_KOMINEK = 40;
            const int OSW_LAZIENKA_LUSTRO = 41;
            const int OSW_SALON = 42;
            const int OSW_KUCHNIA_SZAFKI = 43;

            GroundLights[Resource.Id.switchSalon] = new Light("salon", OSW_SALON, "Salon - główne", new[] { "w salinie", "w salonie duże", "w salonie główne" });
            GroundLights[Resource.Id.switchSalonKominek] = new Light("salon_kominek", OSW_SALON_KOMINEK, "Salon - kominek", new[] { "w salonie przy kominku", "w salonie kominek" });
            GroundLights[Resource.Id.switchSalonLed] = new Light("salon_led", OSW_SALON_LED, "Salon - LED", new[] { "w salonie led", "w salonie ledy" });
            GroundLights[Resource.Id.switchKuchnia] = new Light("kuchnia", OSW_KUCHNIA, "Kuchnia - główne", new[] { "w kuchni", "w kuchni główne" });
            GroundLights[Resource.Id.switchSzafki] = new Light("kuchnia_szafki", OSW_KUCHNIA_SZAFKI, "Kuchnia - blat", new[] { "w kuchni nad blatem" });
            GroundLights[Resource.Id.switchSpizarnia] = new Light("spizarnia", OSW_SPIZARNIA, "Spiżarnia", new[] { "w spiżarni" });
            GroundLights[Resource.Id.switchJadalnia] = new Light("jadalnia", OSW_JADALNIA, "Jadalnia", new[] { "w jadalni" });
            GroundLights[Resource.Id.switchBarek] = new Light("barek", OSW_BAREK, "Barek", new[] { "nad barkiem" });
            GroundLights[Resource.Id.switchGabinet] = new Light("gabinet", OSW_GABINET, "Gabinet", new[] { "w gabinecie" });
            GroundLights[Resource.Id.switchLazienka] = new Light("lazienka", OSW_LAZIENKA, "Łazienka - główne", new[] { "w łazience na dole" });
            GroundLights[Resource.Id.switchLustro] = new Light("lazienka_lustro", OSW_LAZIENKA_LUSTRO, "Łazienka - lustro", new[] { "w łazience na dole przy lustrze" });
            GroundLights[Resource.Id.switchHall] = new Light("hall", OSW_KORYTARZ, "Hall", new[] { "w holu", "w korytarzu na dole", "na korytarzu na dole", "w przejściu na dole" });
            GroundLights[Resource.Id.switchWiatrolap] = new Light("wiatrolap", OSW_WIATROLAP, "Wiatrołap", new[] { "w wiatrołapie" });
            GroundLights[Resource.Id.switchKotlownia] = new Light("kotlownia", OSW_KOTLOWNIA, "Kotłownia", new[] { "w kotłowni" });
            GroundLights[Resource.Id.switchGaraz] = new Light("garaz", OSW_GARAZ, "Garaż", new[] { "w garażu" });
            GroundLights[Resource.Id.switchWejscie] = new Light("wejscie", OSW_WEJSCIE_GLOWNE, "Wejście", new[] { "przy wejściu" });
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