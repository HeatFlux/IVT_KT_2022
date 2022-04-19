using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KonzolInput;
using KevinaZabitChteliNejakaCislaPlzHalp;

namespace KillerNumsGraphic
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class PainWindow : Window
    {
        ViewInputOutput io;
        public PainWindow()
        {
            InitializeComponent();
            io = new ViewInputOutput(getViewBinder(), getOutputView());
            Program process = new Program(io);
            _ = loopProgram(process);
        }
        private async Task loopProgram(Program process)
        {
            do {
                await process.Run();
            } while (true);
        }

        void OnConfirm(Object sender, RoutedEventArgs e)
        {
            io.Confirm();
        }

        private ViewBinder getViewBinder()
        {
            return new ViewBinder(new DataView[] { new BasicInputView(queenT, queenIn), new BasicInputView(numT, numIn) });
        }
        private DataView getOutputView()
        {
            return new BasicLabelView(outputT);
        }
    }

    class ViewInputOutput : InputOutputTask
    {
        private TaskCompletionSource<bool> confirmTask;
        private ViewBinder binder;
        private DataView outputView;
        public ViewInputOutput(ViewBinder binder, DataView outputView)
        {
            this.binder = binder;
            this.outputView = outputView;
        }
        public void Confirm()
        {
            if(confirmTask != null) confirmTask.TrySetResult(true);
        }
        public async Task<bool> GetValueT(IData requestedData)
        {
            throw new NotSupportedException("Method ViewInputOutput/GetValueT does not have a definition");
            /*confirmTask = new TaskCompletionSource<bool>();
            return await confirmTask.Task;*/
        }
        public async Task<bool> GetValuesT(IData[] requestedData)
        {
            binder.insertData(requestedData);
            confirmTask = new TaskCompletionSource<bool>();
            await confirmTask.Task;
            binder.FlushAll();
            return true;
        }
        public async Task<bool> OutStrT(IData outputtedData)
        {
            outputView.SetData(outputtedData);
            return true;
        }
    }
    class ViewBinder
    {
        DataView[] views;
        public ViewBinder(DataView[] views)
        {
            this.views = views;
        }
        public void insertData(IData[] data)
        {
            for (int i = 0; i < views.Length; i++)
            {
                views[i].SetData(data[i]);
            }
        }
        public void FlushAll()
        {
            for (int i = 0; i < views.Length; i++)
            {
                views[i].Flush();
            }
        }
    }
    interface DataView
    {
        bool SetData(IData display);
        bool Flush();
    }
    class BasicInputView : DataView
    {
        IData data;

        TextBlock label;
        TextBox dataInput;
        public BasicInputView(TextBlock label, TextBox dataInput)
        {
            this.label = label;
            this.dataInput = dataInput;
        }
        public bool SetData(IData display)
        {
            data = display;

            Message message = display.GetMessage;
            label.Text = message.GetInput;
            return true;
        }
        public bool Flush()
        {
            data.Process(dataInput.Text);
            return true;
        }
    }
    class BasicLabelView : DataView
    {
        TextBlock output;
        public BasicLabelView(TextBlock output)
        {
            this.output = output;
        }
        public bool SetData(IData display)
        {
            output.Text = display.ToString();
            return true;
        }
        public bool Flush()
        {
            throw new NotSupportedException("Undefined behavior @ BasicLabelView/Flush");
        }
    }
}
