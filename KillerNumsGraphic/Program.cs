using System;
using System.Threading.Tasks;
using KonzolInput;

namespace KevinaZabitChteliNejakaCislaPlzHalp
{
    public class Program
    {
        InputOutputTask io;
        public Program(InputOutputTask io)
        {
            this.io = io;
        }
        public async Task Run()
        {
            Factory factory = new Factory();

            IData[] data = factory.gibData();
            await io.GetValuesT(data);
            int queen = factory.GetKillerQueen;
            int[] numbers = factory.GetKillerNums;

            int pairC = DoubleForSolution(numbers, queen);
            //int pairCE = EnhancedAlgorithm(numbers, queen);

            await io.OutStrT(factory.OutputInt(pairC));
        }
        public static int DoubleForSolution(int[] numbers, int queen)
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
    public class Factory
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
