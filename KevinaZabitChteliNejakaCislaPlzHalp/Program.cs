using System;
using KonzolInput;

namespace KevinaZabitChteliNejakaCislaPlzHalp
{
    class Program
    {
        static void Main(string[] args)
        {
          Console.WriteLine("");
            InputOutput io = new InputOutput();
            Factory factory = new Factory();

            IData[] data = factory.gibData();
            io.GetValues(data);
            int queen = factory.GetKillerQueen;
            int[] numbers = factory.GetKillerNums;

            int pairC = 0;
            for(int i = 0; i < numbers.Length; i++)
            {
                for(int j = i++; j < numbers.Length; j++)
                {
                    if(queen % (i + j) == 0)
                    {
                        pairC++;
                    }
                }
            }

            io.OutStr(factory.OutputInt(pairC));
        }
    }
    class Factory
    {
        private DataI killerQueen;
        private DataArraySplitI killerNums;
        public Factory()
        {

        }
        public IData[] gibData()
        {
            Message messageQueen = new Message(input: "Zadejte královnu");
            killerQueen = new DataI(messageQueen);

            Message minMaxMessage = new Message("Zadali jste moc nízký počet čísel", "Zadali jste moc vysoký počet čísel");
            VibeCheck<int> checkSize = new CheckMinMax(minMaxMessage, 1, 1000000);
            CheckArray<int>[] checkKillerN = new CheckArray<int>[] { new CheckArray<int>(checkSize) };
            Message messageKiller = new Message(input: "Zadejte vražedná čísla oddělená \" \"");
            killerNums = new DataArraySplitI(messageKiller, ' ', checkKillerN);

            return new IData[] { killerQueen, killerNums };
        }
        public int GetKillerQueen
        {
            get { return killerQueen.Value; }
        }
        public int[] GetKillerNums
        {
            get { return killerNums.Value; }
        }
        public DataI OutputInt(int outI)
        {
            DataI data = new DataI(new Message());
            data.Value = outI;
            return data;
        }
    }
}
