/* [grial-metadata] id: Grial#IconText.xaml version: 1.0.1 */
using System;
using System.Collections.Generic;
using UXDivers.Grial;

namespace Barangay_Office
{
    public partial class IconText : ContentView
    {
        public static readonly BindableProperty IconProperty = BindableProperty.Create(
           nameof(Icon),
           typeof(string),
           typeof(IconText));

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly BindableProperty IconHorizontalOptionsProperty = BindableProperty.Create(
            nameof(IconHorizontalOptions),
            typeof(LayoutOptions),
            typeof(IconText),
            defaultValue: LayoutOptions.Center);

        public LayoutOptions IconHorizontalOptions
        {
            get { return (LayoutOptions)GetValue(IconHorizontalOptionsProperty); }
            set { SetValue(IconHorizontalOptionsProperty, value); }
        }

        public static readonly BindableProperty IconVerticalOptionsProperty = BindableProperty.Create(
            nameof(IconVerticalOptions),
            typeof(LayoutOptions),
            typeof(IconText),
            defaultValue: LayoutOptions.Center);

        public LayoutOptions IconVerticalOptions
        {
            get { return (LayoutOptions)GetValue(IconVerticalOptionsProperty); }
            set { SetValue(IconVerticalOptionsProperty, value); }
        }

        public static readonly BindableProperty IconTranslationYProperty = BindableProperty.Create(
            nameof(IconTranslationY),
            typeof(double),
            typeof(IconText),
            defaultValue: 0d);

        public double IconTranslationY
        {
            get { return (double)GetValue(IconTranslationYProperty); }
            set { SetValue(IconTranslationYProperty, value); }
        }

        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
           nameof(IconColor),
           typeof(Color),
           typeof(IconText),
           null);

        public Color IconColor
        {
            get { return (Color)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public static readonly BindableProperty IconFontSizeProperty =
            BindableProperty.Create(
                nameof(IconFontSize),
                typeof(double),
                typeof(IconText),
                defaultValue: 11d);

        public double IconFontSize
        {
            get { return (double)GetValue(IconFontSizeProperty); }
            set { SetValue(IconFontSizeProperty, value); }
        }

        public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(
            nameof(IconFontFamily),
            typeof(string),
            typeof(IconText),
            defaultValue: null);

        public string IconFontFamily
        {
            get { return (string)GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
           nameof(Text),
           typeof(string),
           typeof(IconText));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
           nameof(TextColor),
           typeof(Color),
           typeof(IconText),
           null);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextLineHeightProperty = BindableProperty.Create(
            nameof(TextLineHeight),
            typeof(double),
            typeof(IconText),
            defaultValue: 1d);

        public double TextLineHeight
        {
            get { return (double)GetValue(TextLineHeightProperty); }
            set { SetValue(TextLineHeightProperty, value); }
        }

        public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(
            nameof(TextFontSize),
            typeof(double),
            typeof(IconText),
            defaultValue: 11d);

        public double TextFontSize
        {
            get { return (double)GetValue(TextFontSizeProperty); }
            set { SetValue(TextFontSizeProperty, value); }
        }

        public static readonly BindableProperty TextFontAttributesProperty = BindableProperty.Create(
            nameof(TextFontAttributes),
            typeof(FontAttributes),
            typeof(IconText),
            defaultValue: FontAttributes.None);

        public FontAttributes TextFontAttributes
        {
            get { return (FontAttributes)GetValue(TextFontAttributesProperty); }
            set { SetValue(TextFontAttributesProperty, value); }
        }

        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
           nameof(Orientation),
           typeof(OrientationLayout),
           typeof(IconText),
           propertyChanged: (b, oldValue, newValue) => ((IconText)b).OnOrientationChanged(oldValue, newValue));

        public OrientationLayout Orientation
        {
            get { return (OrientationLayout)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly BindableProperty IconAttributeProperty = BindableProperty.Create(
           nameof(IconAttribute),
           typeof(IconAttribute),
           typeof(IconText),
           propertyChanged: (b, o, n) => ((IconText)b).IconAttributeChanged());

        public IconAttribute IconAttribute
        {
            get { return (IconAttribute)GetValue(IconAttributeProperty); }
            set { SetValue(IconAttributeProperty, value); }
        }

        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
           nameof(Spacing),
           typeof(double),
           typeof(IconText),
           3d,
           propertyChanged: (b, o, n) => ((IconText)b).SpacingChanged());

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static readonly BindableProperty IconBackgroundSizeProperty = BindableProperty.Create(
           nameof(IconBackgroundSize),
           typeof(double),
           typeof(IconText),
           propertyChanged: (b, o, n) => ((IconText)b).UpdateIconBackground());

        public double IconBackgroundSize
        {
            get { return (double)GetValue(IconBackgroundSizeProperty); }
            set { SetValue(IconBackgroundSizeProperty, value); }
        }

        public static readonly BindableProperty IconBackgroundDropShadowProperty = BindableProperty.Create(
           nameof(IconBackgroundDropShadow),
           typeof(DropShadowType),
           typeof(IconText));

        public DropShadowType IconBackgroundDropShadow
        {
            get { return (DropShadowType)GetValue(IconBackgroundDropShadowProperty); }
            set { SetValue(IconBackgroundDropShadowProperty, value); }
        }

        public static readonly BindableProperty IconBackgroundColorProperty = BindableProperty.Create(
           nameof(IconBackgroundColor),
           typeof(Color),
           typeof(IconText),
           propertyChanged: (b, o, n) => ((IconText)b).UpdateIconBackground());

        public Color IconBackgroundColor
        {
            get { return (Color)GetValue(IconBackgroundColorProperty); }
            set { SetValue(IconBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create(
           nameof(LineBreakMode),
           typeof(LineBreakMode),
           typeof(IconText),
           LineBreakMode.CharacterWrap);

        public LineBreakMode LineBreakMode
        {
            get { return (LineBreakMode)GetValue(LineBreakModeProperty); }
            set { SetValue(LineBreakModeProperty, value); }
        }

        private VisualElement _iconBackground, _iconBackgroundColor;

        public IconText()
        {
            InitializeComponent();

            _iconBackground = iconBackground;
            _iconBackgroundColor = DeviceInfo.Platform == DevicePlatform.iOS ? (VisualElement)iconBackgroundIOs : iconBackground;

            OnOrientationChanged(null, OrientationLayout.Horizontal);
            IconAttributeChanged();
            UpdateIconBackground();
        }

        private void OnOrientationChanged(object oldValue, object newValue)
        {
            if (newValue is OrientationLayout newOrientation &&
                (oldValue == null || (oldValue is OrientationLayout oldOrientation && oldOrientation != newOrientation)))
            {
                ClearGridSetValues();
                containerGrid.RowDefinitions.Clear();
                containerGrid.ColumnDefinitions.Clear();

                if (Orientation == OrientationLayout.Horizontal || Orientation == OrientationLayout.InvertedHorizontal)
                {
                    containerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    containerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    containerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    if (Orientation == OrientationLayout.Horizontal)
                    {
                        _iconBackground.SetValue(Grid.ColumnProperty, 0);
                        label.SetValue(Grid.ColumnProperty, 1);
                    }
                    else
                    {
                        _iconBackground.SetValue(Grid.ColumnProperty, 1);
                        label.SetValue(Grid.ColumnProperty, 0);
                    }
                }
                else
                {
                    containerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    containerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    containerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    if (Orientation == OrientationLayout.Vertical)
                    {
                        _iconBackground.SetValue(Grid.RowProperty, 0);
                        label.SetValue(Grid.RowProperty, 1);
                    }
                    else
                    {
                        _iconBackground.SetValue(Grid.RowProperty, 1);
                        label.SetValue(Grid.RowProperty, 0);
                    }
                }

                SpacingChanged();
            }
        }

        private void IconAttributeChanged()
        {
            if (IconAttribute == IconAttribute.Line)
            {
                icon.SetDynamicResource(StyleProperty, "FontIcon");
            }
            else
            {
                icon.SetDynamicResource(StyleProperty, "FontIconFill");
            }
        }

        private void SpacingChanged()
        {
            containerGrid.ColumnSpacing = 0;
            containerGrid.RowSpacing = 0;

            if (Orientation == OrientationLayout.Horizontal || Orientation == OrientationLayout.InvertedHorizontal)
            {
                containerGrid.ColumnSpacing = Spacing;
            }
            else
            {
                containerGrid.RowSpacing = Spacing;
            }            
        }

        private void UpdateIconBackground()
        {
            if (IconBackgroundSize > 0)
            {
                iconBackground.ColumnDefinitions.Clear();
                _iconBackgroundColor.BackgroundColor = IconBackgroundColor;

                _iconBackgroundColor.HeightRequest = IconBackgroundSize;
                _iconBackgroundColor.WidthRequest = IconBackgroundSize;
                _iconBackground.HeightRequest = IconBackgroundSize;
                _iconBackground.WidthRequest = IconBackgroundSize;
                UXDivers.Grial.Effects.SetCornerRadius(_iconBackgroundColor, IconBackgroundSize / 2);
            }
            else
            {
                iconBackground.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                _iconBackgroundColor.BackgroundColor = Colors.Transparent;

                _iconBackgroundColor.HeightRequest = 0;
                _iconBackgroundColor.WidthRequest = 0;

                _iconBackground.ClearValue(VisualElement.WidthRequestProperty);
                _iconBackground.ClearValue(VisualElement.HeightRequestProperty);
                UXDivers.Grial.Effects.SetCornerRadius(_iconBackgroundColor, 0);
            }

            OnOrientationChanged(null, Orientation);
        }

        private void ClearGridSetValues()
        {
            _iconBackground.SetValue(Grid.ColumnProperty, 0);
            label.SetValue(Grid.ColumnProperty, 0);
            _iconBackground.SetValue(Grid.RowProperty, 0);
            label.SetValue(Grid.RowProperty, 0);
        }
    }
}
