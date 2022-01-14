using SmartHome.Model;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome
{
    public class StaticData
    {
        public static Dictionary<int, Light> GroundLights { get; set; }
        public static Dictionary<int, Light> AtticLights { get; set; }
        public static Dictionary<int, Blind> GroundBlinds { get; set; }
        public static Dictionary<int, Blind> AtticBlinds { get; set; }
        public static Dictionary<string, string> LightNames { get; set; }
        public static Dictionary<string, string> BlindNames { get; set; }

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
            AtticBlinds[Resource.Id.buttonAdam1Up] = new Blind("pokoj1_1_up", "Adam lewe");
            AtticBlinds[Resource.Id.buttonAdam1Down] = new Blind("pokoj1_1_down", "Adam lewe");
            AtticBlinds[Resource.Id.buttonAdam2Up] = new Blind("pokoj1_2_up", "Adam prawe");
            AtticBlinds[Resource.Id.buttonAdam2Down] = new Blind("pokoj1_2_down", "Adam prawe");
            AtticBlinds[Resource.Id.buttonUlaUp] = new Blind("pokoj2_up", "Ula");
            AtticBlinds[Resource.Id.buttonUlaDown] = new Blind("pokoj2_down", "Ula");
            AtticBlinds[Resource.Id.buttonSypialniaUp] = new Blind("sypialnia_up", "Sypialnia");
            AtticBlinds[Resource.Id.buttonSypialniaDown] = new Blind("sypialnia_down", "Sypialnia");
        }

        private static void InitializeGroundBlinds()
        {
            GroundBlinds[Resource.Id.buttonSalon1Up] = new Blind("salon1_up", "Salon - taras");
            GroundBlinds[Resource.Id.buttonSalon1Down] = new Blind("salon1_down", "Salon - taras");
            GroundBlinds[Resource.Id.buttonSalon2Up] = new Blind("salon2_up", "Salon - ogród");
            GroundBlinds[Resource.Id.buttonSalon2Down] = new Blind("salon2_down", "Salon - ogród");
            GroundBlinds[Resource.Id.buttonJadalniaUp] = new Blind("jadalnia_up", "Jadalnia");
            GroundBlinds[Resource.Id.buttonJadalniaDown] = new Blind("jadalnia_down", "Jadalnia");
            GroundBlinds[Resource.Id.buttonKotlowniaUp] = new Blind("kotlownia_up", "Kotłownia");
            GroundBlinds[Resource.Id.buttonKotlowniaDown] = new Blind("kotlownia_down", "Kotłownia");
            GroundBlinds[Resource.Id.buttonKuchniaUp] = new Blind("kuchnia_up", "Kuchnia");
            GroundBlinds[Resource.Id.buttonKuchniaDown] = new Blind("kuchnia_down", "Kuchnia");
            GroundBlinds[Resource.Id.buttonGabinetUp] = new Blind("gabinet_up", "Gabinet");
            GroundBlinds[Resource.Id.buttonGabinetDown] = new Blind("gabinet_down", "Gabinet");
            GroundBlinds[Resource.Id.buttonWiatrolapUp] = new Blind("garderoba_up", "Wiatrołap");
            GroundBlinds[Resource.Id.buttonWiatrolapDown] = new Blind("garderoba_down", "Wiatrołap");
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

            AtticLights[Resource.Id.switchSypialnia] = new Light("sypialnia", OSW_SYPIALNIA, "Sypialnia");
            AtticLights[Resource.Id.switchAdam] = new Light("pokoj1", OSW_POKOJ_ADAM, "Adam");
            AtticLights[Resource.Id.switchUla] = new Light("pokoj2", OSW_POKOJ_ULA, "Ula");
            AtticLights[Resource.Id.switchLazienka] = new Light("lazienka_gora", OSW_LAZIENKA_GORA, "Łazienka - główne");
            AtticLights[Resource.Id.switchLazienkaKinkiety] = new Light("lazienka_kinkiety", OSW_LAZIENKA_KINKIETY, "Łazienka - kinkiety");
            AtticLights[Resource.Id.switchLazienkaLed] = new Light("lazienka_led", OSW_LAZIENKA_LED, "Łazienka - LED");
            AtticLights[Resource.Id.switchGarderoba] = new Light("garderoba_gora", OSW_GARDEROBA_GORA, "Garderoba");
            AtticLights[Resource.Id.switchPralnia] = new Light("pralnia", OSW_PRALNIA, "Pralnia");
            AtticLights[Resource.Id.switchKorytarz] = new Light("korytarz", OSW_KORYTARZ, "Korytarz");
            AtticLights[Resource.Id.switchKorytarzLed] = new Light("korytarz_nocne", OSW_KORYTARZ_NOCNE, "Korytarz - nocne");
            AtticLights[Resource.Id.switchSchody] = new Light("schody", OSW_SCHODY, "Schody");
            AtticLights[Resource.Id.switchSchodyLed] = new Light("schody_led", OSW_SCHODY_LED, "Schody - nocne");
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

            GroundLights[Resource.Id.switchSalon] = new Light("salon", OSW_SALON, "Salon - główne");
            GroundLights[Resource.Id.switchSalonKominek] = new Light("salon_kominek", OSW_SALON_KOMINEK, "Salon - kominek");
            GroundLights[Resource.Id.switchSalonLed] = new Light("salon_led", OSW_SALON_LED, "Salon - LED");
            GroundLights[Resource.Id.switchKuchnia] = new Light("kuchnia", OSW_KUCHNIA, "Kuchnia - główne");
            GroundLights[Resource.Id.switchSzafki] = new Light("kuchnia_szafki", OSW_KUCHNIA_SZAFKI, "Kuchnia - blat");
            GroundLights[Resource.Id.switchSpizarnia] = new Light("spizarnia", OSW_SPIZARNIA, "Spiżarnia");
            GroundLights[Resource.Id.switchJadalnia] = new Light("jadalnia", OSW_JADALNIA, "Jadalnia");
            GroundLights[Resource.Id.switchBarek] = new Light("barek", OSW_BAREK, "Barek");
            GroundLights[Resource.Id.switchGabinet] = new Light("gabinet", OSW_GABINET, "Gabinet");
            GroundLights[Resource.Id.switchLazienka] = new Light("lazienka", OSW_LAZIENKA, "Łazienka - główne");
            GroundLights[Resource.Id.switchLustro] = new Light("lazienka_lustro", OSW_LAZIENKA_LUSTRO, "Łazienka - lustro");
            GroundLights[Resource.Id.switchHall] = new Light("hall", OSW_KORYTARZ, "Hall");
            GroundLights[Resource.Id.switchWiatrolap] = new Light("wiatrolap", OSW_WIATROLAP, "Wiatrołap");
            GroundLights[Resource.Id.switchKotlownia] = new Light("kotlownia", OSW_KOTLOWNIA, "Kotłownia");
            GroundLights[Resource.Id.switchGaraz] = new Light("garaz", OSW_GARAZ, "Garaż");
            GroundLights[Resource.Id.switchWejscie] = new Light("wejscie", OSW_WEJSCIE_GLOWNE, "Wejście");
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