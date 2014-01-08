// ----------------------------------------------------------------------------
// <copyright file="InvertableBooleanToVisibilityConverter.cs" company="FPSFA">
//   Copyright © 2013
// </copyright>
// ----------------------------------------------------------------------------
// <author>Poulin Julien - ICT4.2</author>
// <date>06/01/2014</date>
// <project>Hibernator</project>
// ----------------------------------------------------------------------------

namespace Hibernator.Converters
{

  using System;
  using System.Globalization;
  using System.Windows.Data;
  using System.Windows.Markup;

  public class BooleanInverser : MarkupExtension, IValueConverter
  {

    public bool IsInverted { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return this;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return !(bool) value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

  }

}