using System;

namespace KevinovyMagnety
{
    class Program
    {
        static void Main(string[] args)
        {
            Factory factory = new Factory();
            InputOutput io = new InputOutput();
            DataStringArray data = factory.StringArray();

            io.GetValue(data);
            string[] magnetsS = data.Value;

            char lastValue = magnetsS[0][0];
            int units = 1;
            for (int i = 1; i < magnetsS.Length; i++)
            {
                if(magnetsS[i][0] == lastValue)
                {
                    units++;
                }
                else
                {
                    lastValue = magnetsS[i][0];
                }
            }

            DataS str = new DataS();
            str.Process(units.ToString());
            io.OutStr(str);
        }
    }
    class Factory
    {
        public Factory()
        {

        }
        public DataStringArray StringArray()
        {
            return new DataStringArray("Zadejte množství magnetů", "Zadejte orientaci magnetu číslo °",
            "Musí se jednat o celé číslo", checks: new VibeCheck<string[]>[1] { ArrayCheck() } );
        }
        public CheckArrayS ArrayCheck()
        {
            return new CheckArrayS(MinMax(), new VibeCheck<string>[1] { CheckString() });
        }
        public CheckMinMax MinMax()
        {
            return new CheckMinMax(1, 1000000, "Musí se jednat o celé číslo větší než 1", "Musí se jednat o celé číslo menší než 1000000");
        }
        public OneOfStrings CheckString()
        {
            return new OneOfStrings(new string[2] { "+-", "-+" }, "Musí být ve formátu +- či -+");
        }
    }
}
