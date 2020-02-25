using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DNF
{
  
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        internal Matrix matrix { get; set; }

        private void LoadButton(object sender, RoutedEventArgs e)
        {
            int n = Convert.ToInt32(N_Textbox.Text);
            int m = Convert.ToInt32(M_Textbox.Text);
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "Document";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            Nullable<bool> result = dlg.ShowDialog();
            string filename = "";

            if (result == true)
            {

                filename = dlg.FileName;
            }

            if (filename!="") matrix = Matrix.LoadFromFile(n, m, filename);
            if (matrix is null)
            {
                Result_Text_Block.Foreground = new SolidColorBrush(Colors.Red);
                Result_Text_Block.Text = "Błąd wczytywania macierzy";
            }
            else
            {
                Result_Text_Block.Foreground = new SolidColorBrush(Color.FromRgb(63, 204, 41));
                Result_Text_Block.Text = "Macierz wczytana poprawnie";
            }
        }

        private void SolutionButton(object sender, RoutedEventArgs e)
        {
            if (matrix is null) return; 
            List<Trait> list = new List<Trait>();
            int matrixlength = matrix.Value.GetLength(1);
            for (int i = 0; i < matrixlength; i++)
            {
                Trait trait = new Trait(matrix, i);
                if (i == (matrixlength - 1)) trait.Name = "Label";
                list.Add(trait);
            }
           
            Trait label = list.Where(x => x.Name == "Label").FirstOrDefault();
            list.RemoveAt(list.Count - 1);
            List<Trait> list2 = new List<Trait>();
            for (int i = 0; i < matrixlength-1; i++)
            {
                Trait trait = new Trait(matrix, i);
               
                list2.Add(trait);
            }
            Trait labelcopy = new Trait(matrix, matrixlength-1);
            ArrayList Positives = new ArrayList();
            for(int i=0; i<label.Value.Count; i++)
            {
                if (Convert.ToInt32(label.Value[i]) == 1) Positives.Add(i);
            }
            string result = "";
            int z = 0;
            while (Positives.Count != 0)
            {
                
                string r = "";
                ArrayList resultsfromnegatives = new ArrayList();
                ArrayList Negatives = new ArrayList();
                for (int i = 0; i < label.Value.Count; i++)
                {
                    if (Convert.ToInt32(labelcopy.Value[i]) == 0) Negatives.Add(i);
                }
                if (Negatives.Count == 0)
                {
                    result = "Brak negatywóww";
                    break;
                }
                
                ArrayList indexes = new ArrayList();
       
                while (Negatives.Count != 0)
                {

                    double[] quotient = new double[list.Count];
                    for(int i = 0; i < list.Count; i++)
                    {
                        quotient[i] = list[i].PositiveNegativeRatio(label);
                    }
                    foreach (var k in indexes) quotient[Convert.ToInt32(k)] = -1;
                    var maxof = quotient.Max();
                    int index = Array.IndexOf(quotient, maxof);
                    indexes.Add(index);
                   
                    ArrayList ToBeCutOff = list2[index].StrongNegatives(labelcopy);
                
                    for (int i = 0; i < ToBeCutOff.Count; i++)
                    {
                        list.ForEach(x => x.Value[Convert.ToInt32(ToBeCutOff[i])] = 2);
                        label.Value[Convert.ToInt32(ToBeCutOff[i])] = 2;
                      
                    }
                   
                    foreach (var k in ToBeCutOff)
                    {
                        if (Negatives.Contains(k)) Negatives.Remove(k);
                    }

                    if (!(resultsfromnegatives.Contains(index))) resultsfromnegatives.Add(index);
                    r += list[index].Name + "∧";
                    if (ToBeCutOff.Count == 0) break;
                    z++;
                    if (z > 10) break;
                } // koniec wewnętrznej pętli
                
                if (r != "")
                {
                    r = r.Remove(r.Length - 1);
                    r = "(" + r + ")" + "V";
                }
                if (!(result.Contains(r))) result += r;


                ArrayList ToBeCutOffPos = Trait.AllOnes(resultsfromnegatives, list, labelcopy);
               
               
                
               for (int i = 0; i < ToBeCutOffPos.Count; i++)
                    
               {
                    var k =  (char) ToBeCutOffPos[i];
               int ind =  k - 48;
               list.ForEach(x => x.Value[ind] = 2);
               label.Value[ind] = 2;
               }

               
                foreach (var k in ToBeCutOffPos)
                {
                    if (Positives.Contains(k)) Positives.Remove(k);
                }
                if (ToBeCutOffPos.Count == 0) break;
              
            }
            if (result == "") result = "falsee";
            result = result.Remove(result.Length - 1);
            Result_Text_Block.Foreground = new SolidColorBrush(Colors.White);
            Result_Text_Block.Text = result;



        }
    }
}
