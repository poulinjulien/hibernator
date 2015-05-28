namespace Hibernator.Controls
{

  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;

  /// <summary>
  /// Class used to have an image that is able to be grayed when the control is not enabled.
  /// Author: Thomas LEBRUN (http://blogs.developpeur.org/tom).
  /// </summary>
  public class AutoGrayableImage : Image
  {

    private BitmapSource m_OriginalImage;

    private BitmapSource m_GrayedImage;

    private Brush m_GrayedOpacityMask;

    private BitmapSource OriginalImage
    {
      get
      {
        m_OriginalImage = m_OriginalImage ?? (BitmapSource) Source;
        return m_OriginalImage;
      }
    }

    private BitmapSource GrayedImage
    {
      get
      {
        m_GrayedImage = m_GrayedImage ?? new FormatConvertedBitmap(OriginalImage, PixelFormats.Gray32Float, null, 0);
        return m_GrayedImage;
      }
    }

    private Brush GrayedOpacityMask
    {
      get
      {
        m_GrayedOpacityMask = m_GrayedOpacityMask ?? new ImageBrush(OriginalImage);
        return m_GrayedOpacityMask;
      }
    }

    static AutoGrayableImage()
    {
      IsEnabledProperty.OverrideMetadata(typeof(AutoGrayableImage), new FrameworkPropertyMetadata(true, OnAutoGreyScaleImageIsEnabledPropertyChanged));
      SourceProperty.OverrideMetadata(typeof(AutoGrayableImage), new FrameworkPropertyMetadata(null, OnAutoGreyScaleImageSourcePropertyChanged));
    }

    private static void OnAutoGreyScaleImageSourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
    {
      var autoGreyScaleImg = source as AutoGrayableImage;
      if (autoGreyScaleImg != null)
      {
        if (!autoGreyScaleImg.IsEnabled)
        {
          autoGreyScaleImg.Source = autoGreyScaleImg.GrayedImage;
          autoGreyScaleImg.OpacityMask = autoGreyScaleImg.GrayedOpacityMask;
        }
        else
        {
          autoGreyScaleImg.Source = autoGreyScaleImg.OriginalImage;
          autoGreyScaleImg.OpacityMask = null;
        }
      }
    }

    /// <summary>
    /// Called when [auto grey scale image is enabled property changed].
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void OnAutoGreyScaleImageIsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
    {
      var autoGreyScaleImg = source as AutoGrayableImage;
      if (autoGreyScaleImg != null && autoGreyScaleImg.Source != null)
      {
        var isEnable = Convert.ToBoolean(args.NewValue);
        if (!isEnable)
        {
          autoGreyScaleImg.Source = autoGreyScaleImg.GrayedImage;
          autoGreyScaleImg.OpacityMask = autoGreyScaleImg.GrayedOpacityMask;
        }
        else
        {
          autoGreyScaleImg.Source = autoGreyScaleImg.OriginalImage;
          autoGreyScaleImg.OpacityMask = null;
        }
      }
    }

  }

}
