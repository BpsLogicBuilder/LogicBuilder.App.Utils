using LogicBuilder.Domain;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Contoso.Domain
{
    public abstract class EntityModelBase : BaseModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;

            if (eventHandler != null)
            {
                var eventArgs = new PropertyChangedEventArgs(propertyName);

                this.PropertyChanged(this, eventArgs);
            }
        }
    }
}
