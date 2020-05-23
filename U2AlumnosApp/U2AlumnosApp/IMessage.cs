using System;
using System.Collections.Generic;
using System.Text;

namespace U2AlumnosApp
{

    public interface IMessage
    {
        void ShowToast(string text);
        void ShowSnackBar(string text);
        void ShowSnackBar(string text, Action action);

    }
}
