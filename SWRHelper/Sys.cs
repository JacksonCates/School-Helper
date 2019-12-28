using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWRHelper
{
    enum Population { remote, populated };
    enum Loyalty { Empiral, Neutral, Rebel };
    enum TroopType { Ground, Space };
    enum TroopStrength { Triangle, Circle, Square };

    class Sys
    {
        string name;
        Population population;
        Loyalty loyalty;
        Tuple<TroopType, TroopStrength> firstBuildInfo, secondBuildInfo;
        bool hasProbe;

        // Constructor
        public Sys(string name, Population population, Loyalty loyalty,
            TroopType firstTroopType, TroopType secondTroopType, TroopStrength firstTroopStrength, TroopStrength secondTroopStrength)
        {
            this.name = name;
            this.population = population;
            this.loyalty = loyalty;
            hasProbe = false; // Assumes this
            firstBuildInfo = new Tuple<TroopType, TroopStrength>(firstTroopType, firstTroopStrength);
            secondBuildInfo = new Tuple<TroopType, TroopStrength>(secondTroopType, secondTroopStrength);
        }
    }

    // A collection of planets
    class Region
    {
        List<Sys> systems;
        public Region(List<Sys> systems)
        {
            this.systems = systems;
        }
    }
}
