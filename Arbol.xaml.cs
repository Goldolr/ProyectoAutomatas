using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Msagl.Drawing;
using Color = Microsoft.Msagl.Drawing.Color;

namespace AutomatasAFD
{
    /// <summary>
    /// Lógica de interacción para Arbol.xaml
    /// </summary>
    public partial class Arbol : Window
    {
        Graph graph = new Graph("graph");
        Graph graph2 = new Graph("graph2");
        List<String[]> lista = new List<String[]>();
        List<String[]> siguientepos = new List<String[]>();
        List<String[]> Mat = new List<String[]>();
        List<String[]> cabeceras = new List<String[]>();
        Node raiz;
        Node arbol;
        Node raizPP;
        int count = 1;
        String[] firstrowM;


        int contadorId = 0;
        int contadorpos = 1;
        public Arbol()
        {
            InitializeComponent();

        }

        public void llemarM(String nodo)
        {
            String[] rowM = new String[count];
            rowM[0] = nodo;
            String[] nods = nodo.Split(',');
            for (int i = 0; i < nods.Length; i++)
            {
                String sigtNodo = "";
                foreach (String[] sig in siguientepos)
                {
                    if (sig[0].Equals(nods[i]))
                    {
                        sigtNodo = sig[1];
                        break;
                    }
                }

                String vari = "";
                foreach (String[] cab in cabeceras)
                {
                    if (cab[1].Equals(nods[i]))
                    {
                        vari = cab[0];
                    }
                }
                for (int j = 1; j < firstrowM.Length; j++)
                {
                    if (firstrowM[j] != null && firstrowM[j].Equals(vari))
                    {
                        if (rowM[j] == null)
                        {
                            rowM[j] = sigtNodo;
                        }
                        else
                        {
                            rowM[j] += "," + sigtNodo;
                        }
                        rowM[j] = ordenar(rowM[j]);
                    }
                }
            }
            Mat.Add(rowM);
        }

        public String ordenar(String cad)
        {
            if (cad == null || cad.Equals(""))
            {
                return "";
            }
            String r = "";
            String[] vc = cad.Split(',');
            List<String> lisint = new List<String>();
            for (int i = 0; i < vc.Length; i++)
            {
                if (Regex.Match(vc[i] + "", "[0-9]").Success)
                {
                    lisint.Add(vc[i]);
                }
            }
            lisint.Sort();
            List<String> lis = new List<string>();
            foreach (String caract in lisint)
            {
                if (!lis.Contains(caract))
                {
                    r = r + "," + caract;
                    lis.Add(caract);
                }
            }
            r = r.Substring(1);
            return r;
        }

        private Nodo metodoRecursivo(String expresion)
        {
            Nodo arbol = null;
            char[] vec = expresion.ToCharArray();
            for (int i = 0; i < vec.Length; i++)
            {
                if (Regex.Match(vec[i] + "", "([0-9]|[A-Za-z]|λ)").Success)
                {
                    if (!(vec[i] + "").Equals("λ"))
                    {
                        String[] cab = { vec[i] + "", contadorpos + "" };
                        cabeceras.Add(cab);
                    }
                    if (i < vec.Length - 1 && (vec[i + 1] + "").Equals("*"))
                    {
                        Nodo n = new Nodo("*", contadorId++);
                        n.setIzq(new Nodo(vec[i] + "", contadorId++));

                        if ((vec[i] + "").Equals("λ"))
                        {
                            n.getIzq().setEstado(true);
                        }
                        else
                        {
                            n.getIzq().setEstado(false);
                            n.getIzq().setFirst(contadorpos + "");
                            n.getIzq().setLast(contadorpos + "");
                            n.getIzq().setPosicion(contadorpos++);
                            n.setFirst(n.getIzq().getFirst());
                            n.setLast(n.getIzq().getLast());
                        }
                        n.setEstado(true);
                        if (arbol == null)
                        {
                            arbol = n;
                        }
                        else
                        {
                            if (Regex.Match(arbol.getValor(), "(\\||\\.)").Success)
                            {
                                if (arbol.getDer() == null)
                                {
                                    arbol.setDer(n);
                                    if (arbol.getValor().Equals("|"))
                                    {
                                        arbol.setEstado(arbol.getDer().getEstado() || arbol.getIzq().getEstado());
                                        arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                                        arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                                    }
                                    else
                                    {
                                        arbol.setEstado(arbol.getDer().getEstado() && arbol.getIzq().getEstado());
                                        if (arbol.getIzq().getEstado())
                                        {
                                            arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                                        }
                                        else
                                        {
                                            arbol.setFirst(arbol.getIzq().getFirst());
                                        }
                                        if (arbol.getDer().getEstado())
                                        {
                                            arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                                        }
                                        else
                                        {
                                            arbol.setLast(arbol.getDer().getLast());
                                        }
                                    }
                                }
                                else
                                {
                                    Nodo n1 = new Nodo(".", contadorId++);
                                    n1.setIzq(arbol);
                                    n1.setDer(n);
                                    arbol = n1;
                                    arbol.setEstado(arbol.getDer().getEstado() && arbol.getIzq().getEstado());
                                    if (arbol.getIzq().getEstado())
                                    {
                                        arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                                    }
                                    else
                                    {
                                        arbol.setFirst(arbol.getIzq().getFirst());
                                    }
                                    if (arbol.getDer().getEstado())
                                    {
                                        arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                                    }
                                    else
                                    {
                                        arbol.setLast(arbol.getDer().getLast());
                                    }
                                }
                            }
                            else
                            {
                                Nodo n1 = new Nodo(".", contadorId++);
                                n1.setIzq(arbol);
                                n1.setDer(n);
                                arbol = n1;
                                arbol.setEstado(arbol.getDer().getEstado() && arbol.getIzq().getEstado());
                                if (arbol.getIzq().getEstado())
                                {
                                    arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                                }
                                else
                                {
                                    arbol.setFirst(arbol.getIzq().getFirst());
                                }
                                if (arbol.getDer().getEstado())
                                {
                                    arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                                }
                                else
                                {
                                    arbol.setLast(arbol.getDer().getLast());
                                }
                            }
                        }
                        i++;
                    }
                    else
                    {
                        Nodo n = new Nodo(vec[i] + "", contadorId++);
                        if ((vec[i] + "").Equals("λ"))
                        {
                            n.setEstado(true);
                        }
                        else
                        {
                            n.setEstado(false);
                            n.setFirst(contadorpos + "");
                            n.setLast(contadorpos + "");
                            n.setPosicion(contadorpos++);
                        }
                        if (arbol == null)
                        {
                            arbol = n;
                        }
                        else
                        {
                            if (Regex.Match(arbol.getValor(), "(\\||\\.)").Success)
                            {
                                if (arbol.getDer() == null)
                                {
                                    arbol.setDer(n);
                                    if (arbol.getValor().Equals("|"))
                                    {
                                        arbol.setEstado(arbol.getDer().getEstado() || arbol.getIzq().getEstado());
                                        arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                                        arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                                    }
                                    else
                                    {
                                        arbol.setEstado(arbol.getDer().getEstado() && arbol.getIzq().getEstado());
                                        if (arbol.getIzq().getEstado())
                                        {
                                            arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                                        }
                                        else
                                        {
                                            arbol.setFirst(arbol.getIzq().getFirst());
                                        }
                                        if (arbol.getDer().getEstado())
                                        {
                                            arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                                        }
                                        else
                                        {
                                            arbol.setLast(arbol.getDer().getLast());
                                        }
                                    }
                                }
                                else
                                {
                                    Nodo n1 = new Nodo(".", contadorId++);
                                    n1.setIzq(arbol);
                                    n1.setDer(n);
                                    arbol = n1;
                                    arbol.setEstado(arbol.getDer().getEstado() && arbol.getIzq().getEstado());
                                    if (arbol.getIzq().getEstado())
                                    {
                                        arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                                    }
                                    else
                                    {
                                        arbol.setFirst(arbol.getIzq().getFirst());
                                    }
                                    if (arbol.getDer().getEstado())
                                    {
                                        arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                                    }
                                    else
                                    {
                                        arbol.setLast(arbol.getDer().getLast());
                                    }
                                }
                            }
                            else
                            {
                                Nodo n1 = new Nodo(".", contadorId++);
                                n1.setIzq(arbol);
                                n1.setDer(n);
                                arbol = n1;
                                arbol.setEstado(arbol.getDer().getEstado() && arbol.getIzq().getEstado());
                                if (arbol.getIzq().getEstado())
                                {
                                    arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                                }
                                else
                                {
                                    arbol.setFirst(arbol.getIzq().getFirst());
                                }
                                if (arbol.getDer().getEstado())
                                {
                                    arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                                }
                                else
                                {
                                    arbol.setLast(arbol.getDer().getLast());
                                }
                            }
                            if (i < vec.Length - 1 && (vec[i + 1] + "").Equals("("))
                            {
                                String newexp = "";
                                int contparentesis = 0;
                                i++;
                                for (int j = i; j < vec.Length; j++)
                                {
                                    if ((vec[j] + "").Equals("("))
                                    {
                                        contparentesis++;
                                    }
                                    if ((vec[j] + "").Equals(")"))
                                    {
                                        contparentesis--;
                                    }
                                    newexp = newexp + vec[j];
                                    i++;
                                    if (contparentesis == 0)
                                    {
                                        if (j < vec.Length - 1 && (vec[j + 1] + "").Equals("*"))
                                        {
                                            newexp = newexp + vec[j + 1];
                                            i++;
                                        }
                                        i--;
                                        break;
                                    }
                                }
                                Nodo n1 = new Nodo(".", contadorId++);
                                n1.setIzq(arbol);
                                n1.setDer(metodoRecursivo(newexp));
                                arbol = n1;
                                arbol.setEstado(arbol.getDer().getEstado() && arbol.getIzq().getEstado());
                                if (arbol.getIzq().getEstado())
                                {
                                    arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                                }
                                else
                                {
                                    arbol.setFirst(arbol.getIzq().getFirst());
                                }
                                if (arbol.getDer().getEstado())
                                {
                                    arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                                }
                                else
                                {
                                    arbol.setLast(arbol.getDer().getLast());
                                }
                            }
                        }
                    }
                }
                else if ((vec[i] + "").Equals("|"))
                {
                    Nodo n = new Nodo("|", contadorId++);
                    n.setIzq(arbol);
                    if (i < vec.Length - 1 && (vec[i + 1] + "").Equals("("))
                    {
                        String newexp = "";
                        int contparentesis = 0;
                        i++;
                        for (int j = i; j < vec.Length; j++)
                        {
                            if ((vec[j] + "").Equals("("))
                            {
                                contparentesis++;
                            }
                            if ((vec[j] + "").Equals(")"))
                            {
                                contparentesis--;
                            }
                            newexp = newexp + vec[j];
                            i++;
                            if (contparentesis == 0)
                            {
                                if (j < vec.Length - 1 && (vec[j + 1] + "").Equals("*"))
                                {
                                    newexp = newexp + vec[j + 1];
                                    i++;
                                }
                                i--;
                                break;
                            }
                        }
                        n.setDer(metodoRecursivo(newexp));
                    }
                    arbol = n;
                    if (arbol.getDer() != null && arbol.getIzq() != null)
                    {
                        arbol.setEstado(arbol.getDer().getEstado() || arbol.getIzq().getEstado());
                        arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                        arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                    }
                }
                else if (i < vec.Length - 1 && (vec[i] + "").Equals(")") && (vec[i + 1] + "").Equals("("))
                {
                    Nodo n = new Nodo(".", contadorId++);
                    n.setIzq(arbol);
                    i++;
                    String newexp = "";
                    int contparentesis = 0;
                    for (int j = i; j < vec.Length; j++)
                    {
                        if ((vec[j] + "").Equals("("))
                        {
                            contparentesis++;
                        }
                        if ((vec[j] + "").Equals(")"))
                        {
                            contparentesis--;
                        }
                        newexp = newexp + vec[j];
                        i++;
                        if (contparentesis == 0)
                        {
                            if (j < vec.Length - 1 && (vec[j + 1] + "").Equals("*"))
                            {
                                newexp = newexp + vec[j + 1];
                                i++;
                            }
                            i -= 2;
                            break;
                        }
                    }
                    n.setDer(metodoRecursivo(newexp));
                    arbol = n;
                    arbol.setEstado(arbol.getDer().getEstado() && arbol.getIzq().getEstado());
                    if (arbol.getIzq().getEstado())
                    {
                        arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                    }
                    else
                    {
                        arbol.setFirst(arbol.getIzq().getFirst());
                    }
                    if (arbol.getDer().getEstado())
                    {
                        arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                    }
                    else
                    {
                        arbol.setLast(arbol.getDer().getLast());
                    }
                }
                else if (i < vec.Length - 2 && (vec[i] + "").Equals(")") && (vec[i + 1] + "").Equals("*") && (vec[i + 2] + "").Equals("("))
                {
                    Nodo n = new Nodo(".", contadorId++);
                    n.setIzq(new Nodo("*", contadorId++));
                    n.getIzq().setIzq(arbol);
                    n.getIzq().setFirst(arbol.getFirst());
                    n.getIzq().setLast(arbol.getLast());
                    n.getIzq().setEstado(true); ;
                    i += 2;
                    String newexp = "";
                    int contparentesis = 0;
                    for (int j = i; j < vec.Length; j++)
                    {
                        if ((vec[j] + "").Equals("("))
                        {
                            contparentesis++;
                        }
                        if ((vec[j] + "").Equals(")"))
                        {
                            contparentesis--;
                        }
                        newexp = newexp + vec[j];
                        i++;
                        if (contparentesis == 0)
                        {
                            if (j < vec.Length - 1 && (vec[j + 1] + "").Equals("*"))
                            {
                                newexp = newexp + vec[j + 1];
                                i++;
                            }
                            i -= 2;
                            break;
                        }
                    }
                    n.setDer(metodoRecursivo(newexp));
                    arbol = n;
                    arbol.setEstado(arbol.getDer().getEstado() && arbol.getIzq().getEstado());
                    if (arbol.getIzq().getEstado())
                    {
                        arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                    }
                    else
                    {
                        arbol.setFirst(arbol.getIzq().getFirst());
                    }
                    if (arbol.getDer().getEstado())
                    {
                        arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                    }
                    else
                    {
                        arbol.setLast(arbol.getDer().getLast());
                    }
                }
                else if (i < vec.Length - 2 && (vec[i] + "").Equals(")") && (vec[i + 1] + "").Equals("*") && Regex.Match(vec[i + 2] + "", "([0-9]|[A-Za-z]|λ)").Success)
                {
                    Nodo n = new Nodo(".", contadorId++);
                    n.setIzq(new Nodo("*", contadorId++));
                    n.getIzq().setIzq(arbol);
                    n.getIzq().setFirst(arbol.getFirst());
                    n.getIzq().setLast(arbol.getLast());
                    n.getIzq().setEstado(true);
                    arbol = n;
                    /*if (arbol.getIzq().getEstado())
                    {
                        arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
                    }
                    else
                    {
                        arbol.setFirst(arbol.getIzq().getFirst());
                    }
                    if (arbol.getDer().getEstado())
                    {
                        arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
                    }
                    else
                    {
                        arbol.setLast(arbol.getDer().getLast());
                    }*/
                    i++;
                }
                else if (i < vec.Length - 1 && (vec[i] + "").Equals(")") && (vec[i + 1] + "").Equals("*"))
                {
                    Nodo n = new Nodo("*", contadorId++);
                    n.setIzq(arbol);
                    n.setFirst(n.getIzq().getFirst());
                    n.setLast(n.getIzq().getLast());
                    n.setEstado(true);
                    arbol = n;
                    i++;
                }

            }
            return arbol;
        }


        private void ayudanteInorden(Nodo arbol)
        {
            if (arbol == null)
                return;

            Node nod = new Node(arbol.getId() + "");
            Node nod2 = new Node(arbol.getId() + "");
            //nod.LabelText = arbol.getFirst() + "-" + arbol.getValor() + "-" + arbol.getLast();
            nod2.LabelText = arbol.getFirst() + "-" + arbol.getValor() + "-" + arbol.getLast();
            nod.LabelText = arbol.getValor();
            StyleNode(nod, new Color(132, 180, 138));
            graph.AddNode(nod);
            graph2.AddNode(nod2);
            if (arbol.getValor().Equals("*"))
            {
                //String[] nodos = arbol.getLast().Split(',');
                String[] nodos = arbol.getLast().Split(',');
                for (int i = 0; i < nodos.Length; i++)
                {
                    if (!nodos[i].Equals(""))
                    {
                        Boolean exist = false;
                        foreach (string[] fila in siguientepos)
                        {
                            if (fila[0].Equals(nodos[i]))
                            {
                                exist = true;
                                fila[1] = arbol.getFirst() + "," + fila[1];
                                break;
                            }
                        }
                        if (!exist)
                        {
                            String[] newrow = { nodos[i], arbol.getFirst() };
                            siguientepos.Add(newrow);
                        }
                    }
                }
            }
            else if (arbol.getValor().Equals("."))
            {
                String[] nodos = arbol.getIzq().getLast().Split(',');
                for (int i = 0; i < nodos.Length; i++)
                {
                    if (!nodos[i].Equals(""))
                    {
                        Boolean exist = false;
                        foreach (string[] fila in siguientepos)
                        {
                            if (fila[0].Equals(nodos[i]))
                            {
                                exist = true;
                                fila[1] = arbol.getDer().getFirst() + "," + fila[1];
                            }
                        }
                        if (!exist)
                        {
                            String[] newrow = { nodos[i], arbol.getDer().getFirst() };
                            siguientepos.Add(newrow);
                        }
                    }
                }
            }
            if (arbol.getDer() != null)
            {
                String[] aux = { arbol.getId() + "", arbol.getDer().getId() + "" };
                lista.Add(aux);
                ayudanteInorden(arbol.getDer());
            }
            if (arbol.getIzq() != null)
            {
                String[] aux = { arbol.getId() + "", arbol.getIzq().getId() + "" };
                lista.Add(aux);
                ayudanteInorden(arbol.getIzq());
            }
        }

        private static void StyleNode(Node a, Color c)
        {
            // Diseño
            a.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
            a.Label.FontName = "Consolas";
            a.Attr.FillColor = c;
            a.Attr.Color = c;
            a.Label.FontColor = new Microsoft.Msagl.Drawing.Color(255, 255, 255);
            a.Attr.LabelMargin = 4;
        }

        private void CargarAlfabeto_Click(object sender, RoutedEventArgs e)
        {
            Nodo arbol = metodoRecursivo(txtAlfabeto.Text);
            Nodo nraiz = new Nodo(".", contadorId++);
            nraiz.setDer(new Nodo("$", contadorId++));
            nraiz.getDer().setEstado(false);
            nraiz.getDer().setFirst(contadorpos + "");
            nraiz.getDer().setLast(contadorpos + "");
            nraiz.getDer().setPosicion(contadorpos++);
            nraiz.setIzq(arbol);
            arbol = nraiz;
            BtnNuevo.IsEnabled = true;
            if (arbol.getIzq().getEstado())
            {
                arbol.setFirst(arbol.getIzq().getFirst() + "," + arbol.getDer().getFirst());
            }
            else
            {
                arbol.setFirst(arbol.getIzq().getFirst());
            }
            if (arbol.getDer().getEstado())
            {
                arbol.setLast(arbol.getIzq().getLast() + "," + arbol.getDer().getLast());
            }
            else
            {
                arbol.setLast(arbol.getDer().getLast());
            }

            String[] nodos = arbol.getIzq().getLast().Split(',');
            for (int i = 0; i < nodos.Length; i++)
            {
                if (!nodos[i].Equals(""))
                {
                    Boolean exist = false;
                    foreach (string[] fila in siguientepos)
                    {
                        if (fila[0].Equals(nodos[i]))
                        {
                            exist = true;
                            fila[1] = arbol.getDer().getFirst() + "," + fila[1];
                        }
                    }
                    if (!exist)
                    {
                        String[] newrow = { nodos[i], arbol.getDer().getFirst() };
                        siguientepos.Add(newrow);
                    }
                }
            }
            raiz = new Node(arbol.getId() + "");
            raizPP = new Node(arbol.getId() + "");
            //raiz.LabelText = arbol.getFirst() + "-" + arbol.getValor() + "-" + arbol.getLast();
            raizPP.LabelText = arbol.getFirst() + "-" + arbol.getValor() + "-" + arbol.getLast();
            raiz.LabelText = arbol.getValor();
            //raizPP.LabelText = arbol.getValor();
            StyleNode(raiz, new Color(36, 147, 55));
            // StyleNode(raizPP, new Color(55, 147, 55));
            graph.AddNode(raiz);
            graph2.AddNode(raizPP);
            if (arbol.getDer() != null)
            {
                String[] aux = { arbol.getId() + "", arbol.getDer().getId() + "" };
                lista.Add(aux);
                ayudanteInorden(arbol.getDer());
            }
            if (arbol.getIzq() != null)
            {
                String[] aux = { arbol.getId() + "", arbol.getIzq().getId() + "" };
                lista.Add(aux);
                ayudanteInorden(arbol.getIzq());
            }
            foreach (string[] datos in lista)
            {
                graph.AddEdge(datos[0], "", datos[1]);
                graph2.AddEdge(datos[0], "", datos[1]);
            }
            this.txtResultado.AppendText("N" + "\t\t" + "SiguientePos" + "\n");
            foreach (String[] row in siguientepos)
            {
                this.txtResultado.AppendText(row[0] + "\t\t" + row[1] + "\n");
            }

            firstrowM = new String[cabeceras.Count + 1];
            firstrowM[0] = "\t";

            foreach (String[] variables in cabeceras)
            {
                Boolean ex = false;
                for (int j = 0; j < firstrowM.Length; j++)
                {
                    if (firstrowM[j] != null && firstrowM[j].Equals(variables[0]))
                    {
                        ex = true;
                    }
                }
                if (!ex)
                {
                    firstrowM[count++] = variables[0];
                }
            }
            Mat.Add(firstrowM);
            //MessageBox.Show(arbol.getFirst());
            String[] rowM = new String[count];
            rowM[0] = ordenar(arbol.getFirst());
            String[] nods = arbol.getFirst().Split(',');
            for (int i = 0; i < nods.Length; i++)
            {
                String sigtNodo = "";
                foreach (String[] sig in siguientepos)
                {
                    if (sig[0].Equals(nods[i]))
                    {
                        sigtNodo = sig[1];
                    }
                }
                String vari = "";
                foreach (String[] cab in cabeceras)
                {
                    if (cab[1].Equals(nods[i]))
                    {
                        vari = cab[0];
                    }
                }
                for (int j = 0; j < firstrowM.Length; j++)
                {
                    if (firstrowM[j] != null && firstrowM[j].Equals(vari))
                    {
                        if (rowM[j] == null)
                        {
                            rowM[j] = sigtNodo;
                        }
                        else
                        {
                            rowM[j] += "," + sigtNodo;
                        }
                        rowM[j] = ordenar(rowM[j]);
                    }
                }
            }
            Mat.Add(rowM);

            for (int i = 1; i < Mat.Count; i++)
            {
                String[] rows = Mat[i];
                for (int j = 1; j < count; j++)
                {
                    Boolean exist = false;
                    for (int k = 1; k < Mat.Count; k++)
                    {
                        if ((ordenar(Mat[k][0])).Equals(ordenar(rows[j])))
                        {
                            exist = true;
                        }
                    }
                    if (!exist && rows[j] != null)
                    {
                        llemarM(ordenar(rows[j]));
                    }
                }
            }
            this.txtResultado.AppendText("\n");
            this.txtResultado.AppendText("\n");
            this.txtResultado2.AppendText("SiguientePos" + "\t\t\t" + "Estados" + "\n");
            foreach (String[] row in Mat)
            {
                for (int i = 0; i < count; i++)
                {
                    if (row[i] == null)
                    {
                        this.txtResultado2.AppendText("\t\t\t");
                    }
                    else
                    {
                        this.txtResultado2.AppendText(row[i] + "\t\t\t");
                    }
                }
                this.txtResultado2.AppendText("\n\n");
            }
            this.gViewer.Graph = graph;
            this.gViewer2.Graph = graph2;
            BtnAlfabeto.IsEnabled = false;

        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            BtnAlfabeto.IsEnabled = true;
            graph = new Graph("graph");
            graph2 = new Graph("graph2");
            lista = new List<String[]>();
            siguientepos = new List<String[]>();
            Mat = new List<String[]>();
            cabeceras = new List<String[]>();
            count = 1;
            //firstrowM = [];
            txtResultado.Clear();
            txtResultado2.Clear();
            txtAlfabeto.Clear();
            contadorId = 0;
            contadorpos = 1;
            BtnNuevo.IsEnabled = false;
        }
    }
}
