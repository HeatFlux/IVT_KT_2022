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
            return new ViewBinder(new IBindable[] { new BasicInputView(queenT, queenIn), new BasicInputView(numT, numIn) });
        }
        private IBindable getOutputView()
        {
            return new BasicLabelView(outputT);
        }
    }

    class ViewInputOutput : IInputOutputUser
    {
        private TaskCompletionSource<bool> confirmTask;
        private ViewBinder binder;
        private IBindable outputView;
        public ViewInputOutput(ViewBinder binder, IBindable outputView)
        {
            this.binder = binder;
            this.outputView = outputView;
        }
        public void Confirm()
        {
            if(confirmTask != null) confirmTask.TrySetResult(true);
        }
        public async Task<bool> GetValueA(DataView view)
        {
            throw new NotSupportedException("Unsupported behavior @ ViewInputOutput/GetValueA");
            /*confirmTask = new TaskCompletionSource<bool>();
            return await confirmTask.Task;*/
        }
        public async Task<bool> GetValuesA(DataView[] view)
        {
            for (int i = 0; i < view.Length; i++) view[i].Display();

            confirmTask = new TaskCompletionSource<bool>();
            await confirmTask.Task;

            for (int i = 0; i < view.Length; i++) view[i].ReadVal();
            return true;
        }
        public async Task<bool> OutView(DataView view)
        {
            view.Display();
            return true;
        }

        public DataView GetOpenView(IData viewData)
        {
            return binder.BindOpen(viewData);
        }
        public DataView GetMultiView(IData viewData, string[] options)
        {
            throw new NotSupportedException("Unsupported behavior @ ViewInputOutput/GetMultiView");
        }
        public DataView GetOutStrView(IData viewData)
        {
            outputView.Bind(viewData);
            return outputView.ToView();
        }
    }
    class ViewBinder
    {
        int boundViews = 0;
        IBindable[] views;
        public ViewBinder(IBindable[] views)
        {
            this.views = views;
        }
        public DataView BindOpen(IData viewData)
        {
            views[boundViews].Bind(viewData);
            DataView view = views[boundViews].ToView();
            boundViews++;
            return view;
        }
    }
    interface IBindable
    {
        void Bind(IData data);
        DataView ToView();
    }
    class BasicInputView : DataView, IBindable
    {
        IData data;

        TextBlock label;
        TextBox dataInput;
        public BasicInputView(TextBlock label, TextBox dataInput)
        {
            this.label = label;
            this.dataInput = dataInput;
        }
        public void Bind(IData display)
        {
            data = display;
        }
        public DataView ToView()
        {
            return this;
        }
        public bool Display()
        {
            Message message = data.GetMessage;
            label.Text = message.GetInput;
            return true;
        }
        public bool ReadVal()
        {
            data.Process(dataInput.Text);
            return true;
        }
    }
    class BasicLabelView : DataView, IBindable
    {
        IData data;
        TextBlock output;
        public BasicLabelView(TextBlock output)
        {
            this.output = output;
        }
        public void Bind(IData display)
        {
            data = display;
        }
        public DataView ToView()
        {
            return this;
        }
        public bool Display()
        {
            output.Text = data.ToString();
            return true;
        }
        public bool ReadVal()
        {
            throw new NotSupportedException("Unsupported behavior @ BasicLabelView/ReadVal");
        }
    }
}
