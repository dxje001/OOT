using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatV2
{
    internal class Apoteka : INotifyPropertyChanged
    {
        private int godinaOsnivanja;
        private string nazivApoteke;
        private string adresa;
        private string pathSlika;

        private ObservableCollection<Lek> lekovi;
        private ObservableCollection<Radnik> radnici;

        public event PropertyChangedEventHandler PropertyChanged;

        public Apoteka(int godinaOsnivanja, string nazivApoteke, string adresa, string pathSlika)
        {
            this.godinaOsnivanja = godinaOsnivanja;
            this.nazivApoteke = nazivApoteke;
            this.adresa = adresa;
            this.pathSlika = pathSlika;
            Lekovi = new ObservableCollection<Lek>();
            Radnici = new ObservableCollection<Radnik>();
        }

       

        

        public string NazivApoteke
        {
            get { return this.nazivApoteke; }
            set
            {
                if (this.nazivApoteke != value)
                {
                    this.nazivApoteke = value;
                    this.NotifyPropertyChanged("NazivApoteke");
                }
            }
        }

        public int GodinaOsnivanja
        {
            get { return this.godinaOsnivanja; }
            set
            {
                if (this.godinaOsnivanja != value)
                {
                    this.godinaOsnivanja = value;
                    this.NotifyPropertyChanged("GodinaOsnivanja");
                }
            }
        }

        public string Adresa
        {
            get { return this.adresa; }
            set
            {
                if (this.adresa != value)
                {
                    this.adresa = value;
                    this.NotifyPropertyChanged("Adresa");
                }
            }
        }

        public string PathSlika
        {
            get { return this.pathSlika; }
            set
            {
                if (this.pathSlika != value)
                {
                    this.pathSlika = value;
                   this.NotifyPropertyChanged("PathSlika");
                }
            }
        }

        internal ObservableCollection<Lek> Lekovi { get => lekovi; set => lekovi = value; }
        internal ObservableCollection<Radnik> Radnici { get => radnici; set => radnici = value; }
    

        public override string ToString()
        {
            string retVal = "";

            retVal += nazivApoteke + "," + godinaOsnivanja.ToString() + "," + adresa + "," + pathSlika + "\n";

            retVal += String.Join("|", lekovi);

            retVal += "\n";

            retVal += String.Join("|", radnici);

            retVal += "\n";

            return retVal;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
