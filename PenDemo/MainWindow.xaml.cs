using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PenDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StylusPlugIns.Add(new DebugStyluePlugin());
        }

        private void Grid_StylusDown(object sender, StylusDownEventArgs e)
        {
            var stylusPoint = e.GetStylusPoints(this).First();
            var properties = stylusPoint.Description.GetStylusPointProperties();
            foreach (var item in properties)
            {
                var value = stylusPoint.GetPropertyValue(item);
                Console.WriteLine(item.ToString() + "  " + value);
            }

            var device = e.StylusDevice;
            var states = $@"Id = {device.Id}
InAir = {device.InAir}
InRange = {device.InRange}
Inverted = {device.Inverted}
IsValid = {device.IsValid}
NameDevice.Id = {device.TabletDevice.Id}
NameDevice.Name = {device.TabletDevice.Name}
NameDevice.ProductId = {device.TabletDevice.ProductId}
NameDevice.TabletHardwareCapabilities = {device.TabletDevice.TabletHardwareCapabilities}
NameDevice.Type = {device.TabletDevice.Type}
NameDevice.SupportedStylusPointProperties = {string.Join(" | ", device.TabletDevice.SupportedStylusPointProperties.Select(x => $"{FormatGuid(x.Id)}{(x.IsButton ? "&IsButton" : "")}"))}
";

            var raw = GetFieldValue<int[]>(stylusPoint, "_additionalValues");
            var rawData = $"[{string.Join(", ", raw)}]";

            var debug = properties.Select(x => FormatStylusProperty(x, stylusPoint));
            DebugTextBlock.Text = $@"{states}
[{string.Join(", ", raw)}]

{string.Join("\r\n", debug)}";
        }

        private string FormatStylusProperty(StylusPointPropertyInfo info, StylusPoint sp)
        {
            return $@"{FormatGuid(info.Id)}: {sp.GetPropertyValue(info)}
    IsButton={info.IsButton} Maximum={info.Maximum} Minimum={info.Minimum} Resolution={info.Resolution} Unit={info.Unit}";
        }

        private static string FormatGuid(Guid id)
        {
            var name = StylusPointPropertyIds.GetStringRepresentation(id);
            if (name == "Unknown")
            {
                return id.ToString();
            }
            else
            {
                return name;
            }
        }

        private T GetFieldValue<T>(object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var value = (T)field.GetValue(instance);
            return value;
        }
    }

    public class DebugStyluePlugin : StylusPlugIn
    {
        protected override void OnAdded()
        {
            base.OnAdded();
        }

        protected override void OnEnabledChanged()
        {
            base.OnEnabledChanged();
        }

        protected override void OnIsActiveForInputChanged()
        {
            base.OnIsActiveForInputChanged();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
        }

        protected override void OnStylusDown(RawStylusInput rawStylusInput)
        {
            base.OnStylusDown(rawStylusInput);
        }

        protected override void OnStylusDownProcessed(object callbackData, bool targetVerified)
        {
            base.OnStylusDownProcessed(callbackData, targetVerified);
        }

        protected override void OnStylusEnter(RawStylusInput rawStylusInput, bool confirmed)
        {
            base.OnStylusEnter(rawStylusInput, confirmed);
        }

        protected override void OnStylusLeave(RawStylusInput rawStylusInput, bool confirmed)
        {
            base.OnStylusLeave(rawStylusInput, confirmed);
        }

        protected override void OnStylusMove(RawStylusInput rawStylusInput)
        {
            base.OnStylusMove(rawStylusInput);
        }

        protected override void OnStylusMoveProcessed(object callbackData, bool targetVerified)
        {
            base.OnStylusMoveProcessed(callbackData, targetVerified);
        }

        protected override void OnStylusUp(RawStylusInput rawStylusInput)
        {
            base.OnStylusUp(rawStylusInput);
        }

        protected override void OnStylusUpProcessed(object callbackData, bool targetVerified)
        {
            base.OnStylusUpProcessed(callbackData, targetVerified);
        }
    }
}
