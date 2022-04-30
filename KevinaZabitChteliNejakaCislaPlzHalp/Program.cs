using System;
using KonzolInput;

namespace KevinaZabitChteliNejakaCislaPlzHalp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");
            IInputOutputUser io = new IOConsoleUser();
            Factory factory = new Factory(io);

            DataView[] data = factory.gibData();
            io.GetValuesA(data).Wait();
            int queen = factory.GetKillerQueen;
            int[] numbers = factory.GetKillerNums;

            int pairC = DoubleForSolution(numbers, queen);
            //int pairCE = EnhancedAlgorithm(numbers, queen);

            io.OutView(factory.OutputInt(pairC)).Wait();

            Console.ReadLine();
        }
        static int DoubleForSolution(int[] numbers, int queen)
        {
            int pairC = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                for (int j = i + 1; j < numbers.Length; j++)
                {
                    if ((numbers[i] + numbers[j]) % queen == 0)
                    {
                        pairC++;
                    }
                }
            }

            return pairC;
        }
        /*static int EnhancedAlgorithm(int[] numbers, int queen)
        {
            int[] modules = new int[queen];
            for (int i = 0; i < modules.Length; i++)
            {
                modules[i] = 0;
            }

            for (int i = 0; i < numbers.Length; i++)
            {
                modules[numbers[i] % queen] += 1;
            }

            int pairC = 0;
            //if (modules.Length > 0) pairC += ;
            int halfLenght = numbers.Length / 2;
            for (int i = 0; i < halfLenght; i++)
            {
                pairC += modules[i] * modules[modules.Length -1 -i];
            }
            if ((queen % 2) == 0)
            {
                int midModule = modules[modules.Length / 2 + 1];
                if (midModule > 1) pairC += midModule / 2;
            }
            return pairC;
        }*/
    }
    class Factory
    {
        IInputOutputUser io;
        private DataI killerQueen;
        private DataArraySplitI killerNums;
        public Factory(IInputOutputUser io)
        {
            this.io = io;
        }
        public DataView[] gibData()
        {
            Message messageQueen = new Message(input: "Zadejte královnu");
            killerQueen = new DataI(messageQueen);

            Message minMaxMessage = new Message("Zadali jste moc nízký počet čísel", "Zadali jste moc vysoký počet čísel");
            VibeCheck<int> checkSize = new CheckMinMax(minMaxMessage, 1, 1000000);
            CheckArray<int>[] checkKillerN = new CheckArray<int>[] { new CheckArray<int>(checkSize) };
            Message messageKiller = new Message(input: "Zadejte vražedná čísla oddělená \" \"");
            killerNums = new DataArraySplitI(messageKiller, ' ', checkKillerN);

            return new DataView[] { io.GetOpenView(killerQueen), io.GetOpenView(killerNums) };
        }
        public int GetKillerQueen
        {
            get { return killerQueen.Value; }
        }
        public int[] GetKillerNums
        {
            get { return killerNums.Value; }
        }
        public DataView OutputInt(int outI)
        {
            DataI data = new DataI(new Message());
            data.Value = outI;
            return io.GetOutStrView(data);
        }
    }
}
