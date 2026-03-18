using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisremaDeGestion.View_Model
{
    public abstract class ViewModelBase: INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;

    }
}
