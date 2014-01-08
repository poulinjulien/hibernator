namespace Hibernator.Converters
{

  using System;
  using System.Globalization;
  using System.Windows.Data;
  using System.Windows.Markup;
  using Humanizer;

  public class TimeSpanHumanizer : MarkupExtension, IValueConverter
  {

    public TimeSpanHumanizer()
    {
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return this;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is TimeSpan)
      {
        var timeSpan = (TimeSpan) value;

        return timeSpan.Humanize();
      }

      return "???";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

  }

}