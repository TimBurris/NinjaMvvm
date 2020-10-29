using FluentValidation.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NinjaMvvm.Wpf
{

    /// <summary>
    /// provides a Wpf Specific implementation of ViewModelBase
    /// </summary>
    public abstract class WpfViewModelBase : ViewModelBase, System.ComponentModel.IDataErrorInfo
    {

        private static bool? _isDesignMode;
        /// <summary>
        /// Returns whether or not Wpf is running in design mode
        /// </summary>
        /// <returns></returns>
        protected override bool IsInDesignMode()
        {

            if (!_isDesignMode.HasValue)
                _isDesignMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());

            return _isDesignMode.Value;
        }

        #region Validation and IDataErrorInfo
        /// <summary>
        /// Override this to provide a type specific IValidator to be used with <see cref="GetValidationResult"/>.  If not overridden, validation will simply always pass.
        /// </summary>
        /// <returns></returns>
        protected virtual FluentValidation.IValidator GetValidator() { return null; }

        /// <summary>
        /// Specifies whether or not the IDataErrorInfo impementation will be filled with error data (thereby determining whether or not they get displayed)
        /// </summary>
        public bool ShowErrors
        {
            get { return GetField<bool>(); }
            set
            {
                var changed = SetField(value);
                if (changed && value)
                {
                    //once showErrors gets turned on, we have to fire onchanged events for all properties that have errors.  this is to force WPF to rebind and check for errors that are now there
                    this.GetValidator()
                        ?.Validate(new FluentValidation.ValidationContext<WpfViewModelBase>(this))
                        ?.Errors
                        ?.ToList()
                        ?.ForEach(error => this.OnPropertyChanged(propertyName: error.PropertyName));
                }
            }
        }

        /// <summary>
        /// Performs Validation of the ViewMode using the <see cref="GetValidator"/>
        /// </summary>
        /// <returns></returns>
        public FluentValidation.Results.ValidationResult GetValidationResult()
        {
            return GetValidator()?.Validate(new FluentValidation.ValidationContext<WpfViewModelBase>(this));
        }

        #region IDataErrorInfo
        /// <summary>
        /// Implementation of <see cref="System.ComponentModel.IDataErrorInfo.Error"/>
        /// </summary>
        public string Error
        {
            get { return null; }
        }
        /// <summary>
        /// Implementation of <see cref="System.ComponentModel.IDataErrorInfo"/>
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                if (!this.ShowErrors)
                    return null;

                var validator = GetValidator();
                if (validator == null)
                    return null;

                //using validationContext we can tell FluentValidation to only validatie the field specified
                var context = new FluentValidation.ValidationContext<WpfViewModelBase>(this,
                     new FluentValidation.Internal.PropertyChain(),
                     new FluentValidation.Internal.MemberNameValidatorSelector(new string[] { columnName }));

                var result = validator.Validate(context);

                if (result?.Errors?.Any() ?? false)
                {
                    return string.Join(Environment.NewLine, result.Errors.Select(e => e.ErrorMessage).ToArray());
                }
                else
                    return null;

            }
        }
        #endregion
        #endregion
    }
}
