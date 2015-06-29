namespace Gu.Wpf.Validation
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    using Gu.Wpf.Validation.Internals;

    /// <summary>
    /// http://tech.pro/tutorial/856/wpf-tutorial-using-a-visual-collection
    /// </summary>
    public class ContentAdorner : Adorner
    {
        private readonly VisualCollection _children;
        private readonly ContentPresenter _contentPresenter;

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content",
            typeof(object), 
            typeof(ContentAdorner),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty DataTemplateProperty = DependencyProperty.Register(
            "DataTemplate",
            typeof(DataTemplate),
            typeof(ContentAdorner),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty DataTemplateSelectorProperty = DependencyProperty.Register(
            "DataTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(ContentAdorner),
            new PropertyMetadata(default(DataTemplateSelector)));

        static ContentAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentAdorner), new FrameworkPropertyMetadata(typeof(ContentAdorner)));
        }

        public ContentAdorner(UIElement adornedElement, object content)
            : base(adornedElement)
        {
            _children = new VisualCollection(this);
            _contentPresenter = new ContentPresenter();
            _children.Add(_contentPresenter);
            BindingHelper.Bind(_contentPresenter, MarginProperty, this, MarginProperty);
            BindingHelper.Bind(_contentPresenter, ContentPresenter.ContentProperty, this, ContentProperty);
            BindingHelper.Bind(_contentPresenter, ContentPresenter.ContentTemplateProperty, this, DataTemplateProperty);
            BindingHelper.Bind(_contentPresenter, ContentPresenter.ContentTemplateSelectorProperty, this, DataTemplateSelectorProperty);
            Content = content;
        }

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public DataTemplate DataTemplate
        {
            get { return (DataTemplate)GetValue(DataTemplateProperty); }
            set { SetValue(DataTemplateProperty, value); }
        }

        public DataTemplateSelector DataTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(DataTemplateSelectorProperty); }
            set { SetValue(DataTemplateSelectorProperty, value); }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _contentPresenter.Measure(constraint);
            return constraint;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _contentPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return _contentPresenter.RenderSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return _contentPresenter;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
    }
}
