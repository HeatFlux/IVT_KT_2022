using System;
using System.Text;
using System.Collections.Generic;

namespace KonzolInput
{
    static class Program
    {
        static void Main(string[] args)
        {
            InputOutput input = new InputOutput();
            //input.GetValue(new DataStringArray(checks: new VibeCheck<string[]>[1] { new CheckArray<string>(new CheckMinMax(1, 100)) }));
            DataArraySplitS data = new DataArraySplitS(new Message(),checks: new VibeCheck<IEnumerable<string>>[]{
            new CheckArray<string>(checks: new CheckStringFirstChar[]
            {
                new CheckStringFirstChar(new Message("Zadaný text musí začínat na a"), 'a')
            }
            , sizeCheck: new CheckMinMax(new Message(), 0, 10))});
            input.GetValue(data);
            Console.WriteLine(data.Value.Length.ToString());
            Console.WriteLine(data.ToString());
        }
    }
    public class InputOutput
    {
        public InputOutput()
        {

        }
        public bool GetValue(IData localData)
        {
            bool success;
            Message msg = localData.GetMessage;
            do
            {
                Console.Write(msg.GetInput + ": ");
                success = localData.Process(Console.ReadLine());
                if (!success)
                {
                    Console.WriteLine(msg.GetError);
                }
            } while (!success);
            return true;
        }
        public bool GetValues(IData[] localData)
        {
            foreach (IData data in localData)
            {
                GetValue(data);
            }
            return true;
        }
        public bool OutStr(IData localData)
        {
            Console.WriteLine(localData.ToString());
            return true;
        }
    }
    /*public interface IMessage
    {
        string _input { get; set; }
        string _parseF { get; set; }
        string _toolTip { get; set; }
        string _help { get; set; }
        StringBuilder _error { get; set; }
    }*/
    public class Message// : IMessage
    {
        private string input;

        private StringBuilder _error { get; set; }
        private string error;
        private string special;

        public string _toolTip { get; set; }
        public string _help { get; set; }
        public Message(string error = "Oya; oya, oya?", string input = "Zadejte hodnotu",
            string toolTip = "null", string help = "null", string special = null)
        {
            this.input = input;
            this.error = error;
            this.special = special;
            _toolTip = toolTip;
            _help = help;
            _error = new StringBuilder();
            _error.Append(error);
        }
        public string GetInput
        {
            get { return input; }
        }
        public string GetError
        {
            get { return _error.ToString(); }
        }
        public void ChangeError(int index)
        {
            switch (index)
            {
                case 0:
                    ChangeError(error);
                    break;
                case -1:
                    if (special != null)
                    {
                        ChangeError(special);
                    }
                    else
                    {
                        ChangeError(error);
                    }
                    break;
                default:
                    _error.Clear();
                    break;
            }
        }
        public void ChangeError(string newError)
        {
            _error.Clear();
            _error.Append(newError);
        }
    }
    public interface IData
    {
        bool Process(string text);
        Message GetMessage { get; }
    }
    /// <summary>
    /// Data int
    /// </summary>
    public class DataI : IData
    {
        int _dato;
        VibeCheck<int>[] checks;
        Message msg;
        public DataI(Message msg, VibeCheck<int>[] checks = null)
        {
            this.checks = checks;
            this.msg = msg;
        }
        public bool Process(string txt)
        {
            bool success = int.TryParse(txt, out _dato);
            if (success)
            {
                if (checks != null)
                {
                    for (int i = 0; i < checks.Length; i++)
                    {
                        if (!checks[i].QuickLook(_dato))
                        {
                            success = false;
                            msg.ChangeError(checks[i].Error);
                            break;
                        }
                    }
                }
            }
            else
            {
                msg.ChangeError(0);
            }
            return success;
        }
        public override string ToString()
        {
            return _dato.ToString();
        }
        public Message GetMessage
        {
            get { return msg; }
        }
        public int Value
        {
            get { return _dato; }
            set { _dato = value; }
        }
    }
    /// <summary>
    /// Data string
    /// </summary>
    public class DataS : IData
    {
        string _dato;
        VibeCheck<string>[] checks;
        Message msg;
        public DataS(Message msg, VibeCheck<string>[] checks = null)
        {
            this.checks = checks;
            this.msg = msg;
        }
        public bool Process(string txt)
        {
            _dato = txt;
            bool success = true;

            if (checks != null)
            {
                for (int i = 0; i < checks.Length; i++)
                {
                    if (!checks[i].QuickLook(_dato))
                    {
                        msg.ChangeError(checks[i].Error);
                        success = false;
                        break;
                    }
                }
            }
            return success;
        }
        public override string ToString()
        {
            return _dato.ToString();
        }
        public Message GetMessage
        {
            get { return msg; }
        }
        public string Value
        {
            get { return _dato; }
        }
    }
    public class DataArraySplitI : IData
    {
        const string defaultInput = "Zadejte pole * int, oddělených \"";
        int[] _data;
        VibeCheck<IEnumerable<int>>[] checks;
        Message msg;
        char splitChar;
        public DataArraySplitI(Message msg, char splitChar = '|', VibeCheck<IEnumerable<int>>[] checks = null)
        {
            this.checks = checks;
            this.splitChar = splitChar;
            this.msg = msg;
        }
        public bool Process(string txt)
        {
            bool success = false;
            string[] inputData = txt.Split(splitChar);
            _data = new int[inputData.Length];
            for (int i = 0; i < inputData.Length; i++)
            {
                if (!int.TryParse(inputData[i], out _data[i]))
                {
                    success = false;
                    msg.ChangeError(0);
                    break;
                }
            }
            if (checks != null)
            {
                success = true;
                for (int i = 0; i < checks.Length; i++)
                {
                    if (!checks[i].QuickLook(_data))
                    {
                        success = false;
                        msg.ChangeError(checks[i].Error);
                        break;
                    }
                }
            }

            return success;
        }
        public override string ToString()
        {
            string outStr = _data[0].ToString();
            for (int i = 1; i < _data.Length; i++)
            {
                outStr += "\n" + _data[i];
            }
            return outStr;
        }
        public Message GetMessage
        {
            get { return msg; }
        }
        public int[] Value
        {
            get { return _data; }
        }
    }
    public class DataArraySplitS : IData
    {
        const string defaultInput = "Zadejte pole string, oddělených \"";
        string[] _data;
        VibeCheck<IEnumerable<string>>[] checks;
        Message msg;
        char splitChar;
        public DataArraySplitS(Message msg, char splitChar = '|', VibeCheck<IEnumerable<string>>[] checks = null)
        {
            this.checks = checks;
            this.splitChar = splitChar;
            this.msg = msg;
        }
        public bool Process(string txt)
        {
            _data = txt.Split(splitChar);
            bool success = true;
            if (success)
            {
                for (int i = 0; i < checks.Length; i++)
                {
                    if (!checks[i].QuickLook(_data))
                    {
                        msg.ChangeError(checks[i].Error);
                        success = false;
                        break;
                    }
                }
            }

            return success;
        }
        public override string ToString()
        {
            string outStr = _data[0];
            for(int i = 1; i < _data.Length; i++)
            {
                outStr += "\n" + _data[i];
            }
            return outStr;
        }
        public Message GetMessage
        {
            get { return msg; }
        }
        public string[] Value
        {
            get { return _data; }
        }
    }
    /*public class DataStringArray : IData
    {
        VibeCheck<IEnumerable<string>>[] checks;
        Message msg;
        string[] _data;
        int currentDato;
        string inputM;
        public DataStringArray(Message msg, VibeCheck<IEnumerable<string>>[] checks = null)
        {
            this.checks = checks;
            currentDato = -1;
        }
        public bool Process(string inTxt)
        {
            bool success;
            if (currentDato == -1)
            {
                int aSize;
                success = int.TryParse(inTxt, out aSize);
                if (success)
                {
                    _data = new string[aSize];
                    if (checks != null)
                    {
                        for (int x = 0; x < checks.Length; x++)
                        {
                            if (!checks[x].QuickLook(_data))
                            {
                                msg.ChangeError(checks[x].Error);
                                success = false;
                                break;
                            }
                        }
                    }
                    if (success)
                    {
                        currentDato = 0;
                        msg.ChangeError(-1);
                        ChangeMessageNumber(1);
                        if (currentDato != _data.Length)
                        {
                            success = false;
                        }
                    }
                }
                else
                {
                    msg.ChangeError(0);
                }
            }
            else
            {
                success = true;
                _data[currentDato] = inTxt;
                if (checks != null)
                {
                    for (int x = 0; x < checks.Length; x++)
                    {
                        if (!checks[x].QuickLook(_data))
                        {
                            msg.ChangeError(checks[x].Error);
                            success = false;
                            break;
                        }
                    }
                }
                if (success)
                {
                    if (currentDato != _data.Length - 1)
                    {
                        success = false;
                        int messageNumber = currentDato + 2;
                        ChangeMessageNumber(currentDato + 2);
                        currentDato++;
                    }
                }
            }
            return success;
        }
        void ChangeMessageNumber(int messageNumber)
        {
            string[] temp = inputM.ToString().Split('°');
            _error.Clear();
            _error.Append(temp[0]);
            _error.Append(messageNumber);
            for (int i = 1; i < temp.Length; i++)
            {
                _error.Append(temp[i]);
            }
        }
        /*public override string ToString()
        {
            return _dato.ToString();
        }* /
        public string[] Value
        {
            get { return _data; }
        }
    }*/
    public interface VibeCheck<T>
    {
        public bool QuickLook(T dato);
        public string Error { get; }
    }
    public class CheckMinMax : VibeCheck<int>
    {
        Message msg;
        int min;
        int max;
        /// <param name="msg">error for minError, special for maxError</param>
        public CheckMinMax(Message msg, int min = int.MinValue, int max = int.MaxValue)
        {
            this.min = min;
            this.max = max;
            this.msg = msg;
        }
        public bool QuickLook(int dato)
        {
            bool splnuje = true;
            if (dato < min)
            {
                splnuje = false;
                msg.ChangeError(0);
            }
            else if (dato > max)
            {
                splnuje = false;
                msg.ChangeError(-1);
            }
            return splnuje;
        }
        public string Error
        { get { return msg.GetError; } }
    }
    public class CheckStringLenghtMinMax : VibeCheck<string>
    {
        Message msg;
        string _ErrorM;
        int min;
        int max;
        /// <param name="msg">error for minError, special for maxError</param>
        public CheckStringLenghtMinMax(Message msg, int min = 0, int max = int.MaxValue)
        {
            this.min = min;
            this.max = max;
            this.msg = msg;
        }
        public bool QuickLook(string dato)
        {
            bool splnuje = true;
            if (dato.Length < min)
            {
                splnuje = false;
                msg.ChangeError(0);
            }
            else if (dato.Length > max)
            {
                splnuje = false;
                msg.ChangeError(-1);
            }
            return splnuje;
        }
        public string Error
        { get { return msg.GetError; } }
    }
    public class CheckStringFirstChar : VibeCheck<string>
    {
        Message msg;
        char intendedchar;
        public CheckStringFirstChar(Message msg, char intendedchar)
        {
            this.intendedchar = intendedchar;
            this.msg = msg;
        }
        public bool QuickLook(string dato)
        {
            bool splnuje = false;
            if (dato.Length != 0)
            {
                if (dato[0] == intendedchar)
                {
                    splnuje = true;
                }
            }
            return splnuje;
        }
        public string Error
        { get { return msg.GetError; } }
    }
    public class OneOfStrings : VibeCheck<string>
    {
        Message msg;
        string[] strings;
        public OneOfStrings(Message msg, string[] strings, string error)
        {
            this.strings = strings;
            this.msg = msg;
        }
        public bool QuickLook(string txt)
        {
            bool good = false;
            for (int i = 0; i < strings.Length; i++)
            {
                if(txt == strings[i])
                {
                    good = true;
                    break;
                }
            }
            return good;
        }
        public string Error
        { get { return msg.GetError; } }
    }
    public class CheckArray <T> : VibeCheck<IEnumerable<T>>
    {
        string _error;
        VibeCheck<int> sizeCheck;
        VibeCheck<T>[] checks;
        public CheckArray(VibeCheck<int> sizeCheck = null, VibeCheck<T>[] checks = null)
        {
            this.sizeCheck = sizeCheck;
            this.checks = checks;
        }
        public bool QuickLook(IEnumerable<T> input)
        {
            bool success = true;
            bool check = false;
            int lenght = 0;
            if (checks != null)
            {
                check = true;
            }
            foreach (T item in input)
            {
                if (check)
                {
                    for (int x = 0; x < checks.Length; x++)
                    {
                        if (!checks[x].QuickLook(item))
                        {
                            _error = checks[x].Error;
                            success = false;
                            break;
                        }
                    }
                    if (!success)
                    {
                        break;
                    }
                }
                lenght++;
            }
            if (sizeCheck != null && success)
            {
                if (!sizeCheck.QuickLook(lenght))
                {
                    _error = sizeCheck.Error;
                    success = false;
                }
            }
            if (success)
            {
                
            }
            return success;
        }
        public string Error
        { get { return _error; } }
    }
}