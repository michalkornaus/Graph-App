using System.Diagnostics;

namespace GraphApp
{
    public partial class Form1 : Form
    {
        // Tablica przechowuj�ca wszystkie istniej�ce wierzcho�ki 
        private List<Vertex> ListOfVertices = new List<Vertex>();
        // Zmienne pomocnicze do algorytmu sprawdzaj�cego czy graf jest cykliczny
        private Vertex? vertex_buffor = null;
        private bool found_cycle = false;
        private Vertex? last_vertex = null;
        // Zmienna Actions zawiera 3 dost�pne opcje/akcje kt�re u�ytkownik mo�e wykona� na wierzcho�kach,
        // Domy�lnie program rozpoczyna z zaznaczon� opcj� Preview - czyli Wybieraniem wierzcho�ka
        enum Actions { PREVIEW, ADD, CONNECT }
        private Actions action = Actions.PREVIEW;
        public Form1()
        {
            InitializeComponent();
        }
        // Funkcja inicjuj�ca przeszukanie grafu wg��b w poszukiwaniu cyklu - bierze obecny wierzcho�ek, zmienn� Dictionary i list� wierzcho�k�w cyklicznych jako parametry
        private bool cyclic_helper(Vertex current, Dictionary<Vertex, bool> visited, List<Vertex> cyclicVert)
        {
            // Oznacza obecny wierzcho�ek jako odwiedzony - aby funkcja wiedzia�a kiedy sko�czy� przeszukiwanie grafu
            visited[current] = true;
            // Przeszukiwane s� wszystkie po��czenia wierzcho�ka
            foreach (Vertex vertex in current.GetConnections())
            {
                // Sprawdzane zostaje czy dany po��czony wierzcho�ek by� ju� wcze�niej przeszukiwany
                if (!visited[vertex])
                {
                    // Je�eli wierzcho�ek nie by� odwiedzany, to wywo�uje si� funkcja kontynuuj�ca przeszukiwanie grafu od tego miejsca wg��b
                    if (cyclic_helper(vertex, visited, current, cyclicVert))
                    {
                        // Je�eli powy�sza funkcja przeszukuj�ca graf zwr�ci�a warto�� true - to znaczy �e odnalaz�a cykl - dodany zostaje obecny wierzcho�ek do listy i zwracana warto�� true
                        if (!cyclicVert.Contains(current) && !found_cycle)
                            cyclicVert.Add(current);
                        return true;
                    }
                }
            }
            return false;
        }
        // Funkcja kontynuuj�ca przeszukanie grafu wg�ab w poszkuwaniu cyklu - bierze jeden wi�cej parametr ni� powy�sza funkcja: Vertex parent, aby wiedzie� od kt�rego wierzcho�ka wywo�a�a si� ta funkcja 
        private bool cyclic_helper(Vertex current, Dictionary<Vertex, bool> visited, Vertex parent, List<Vertex> cyclicVert)
        {
            // Oznacza obecny wierzcho�ek jako odwiedzony
            visited[current] = true;
            // Przeszukiwane s� wszystkie po��czenia wierzcho�ka
            foreach (Vertex vertex in current.GetConnections())
            {
                // Sprawdzane zostaje czy dany po��czony wierzcho�ek by� ju� wcze�niej przeszukiwany
                if (!visited[vertex])
                {
                    // Je�eli po��czony wierzcho�ek nie by� odwiedzany, to wywo�uje si� funkcja kontynuuj�ca przeszukiwanie grafu od tego miejsca wg��b
                    if (cyclic_helper(vertex, visited, current, cyclicVert))
                    {
                        // Je�eli powy�sza funkcja przeszukuj�ca graf zwr�ci�a warto�� true - to znaczy �e odnalaz�a cykl - dodany zostaje obecny wierzcho�ek do listy
                        if (!cyclicVert.Contains(current) && !found_cycle)
                            cyclicVert.Add(current);
                        if (current == last_vertex)
                            // Je�eli obecny wierzcho�ek jest r�wny ostatniemu wierzcho�kowi w cyklu to zmienia zmienn� found_cycle na true -
                            // - jest to zmienna pomocnicza aby m�c wy�wietla� po przeszukaniu grafu odpowiednie wierzcho�ki sk�adaj�ce si� na cykl, bez niej przez rekurencyjne podej�cie by�oby zbyt du�o wierzcho�k�w wpisanych do listy
                            found_cycle = true;
                        return true;
                    }
                }
                // Je�eli dany po��czony wierzcho�ek by� ju� przeszukiwany wcze�niej, to sprawdzamy czy dany wierzcho�ek nie jest wierzcho�kiem poprzedzaj�cym przeszukiwanie
                else if (vertex != parent)
                {
                    // Je�eli dotarli�my do wierzcho�ka kt�ry by� ju� odwiedzany i nie jest poprzedzaj�cym wierzcho�kiem to znaczy �e odnale�li�my cykl w grafie

                    // Dodajemy do listy cyklicznych wierzcho�k�w obecny wierzcho�ek - je�eli nie jest ju� na li�cie
                    if (!cyclicVert.Contains(current))
                        cyclicVert.Add(current);
                    // Zapisujemy do zmiennej pomocniczej last_vertex czyli ostatni wierzcho�ek w cyklu, tj. znaleziony po��czony wierzcho�ek
                    last_vertex = vertex;
                    // Zwracamy warto�� true, poniewa� cykl zosta� znaleziony
                    return true;
                }
            }
            // Zwracamy warto�� false, je�eli ka�dy przeszukany po��czony wierzcho�ek nie mia� innych po��cze�
            return false;
        }

        private bool check_graph_cyclic()
        {
            found_cycle = false;
            label3.Text = "";
            if (ListOfVertices.Count < 3)
            {
                ShowTooltip("Nie mo�na sprawdzi� cykliczno�ci grafu dla mniej ni� trzech wierzcho�k�w!");
                return false;
            }
            // Utworzenie zmiennej Dictionary (s�ownika) - przyjmuj�cej po��czone pary: wierzcho�ka ze zmienn� bool;
            // Dzi�ki temu mo�emy mie� list� wszystkich wierzcho�k�w i odpowiadaj�cy im stan true/false, tj. czy zosta�y ju� przeszukane
            Dictionary<Vertex, bool> visitDict = new Dictionary<Vertex, bool>();
            // Lista wierzcho�k�w tworz�cych cykl w grafie
            List<Vertex> cyclicVertices = new List<Vertex>();
            foreach (Vertex vertex in ListOfVertices)
            {
                // Dodajemy wszystkie utworzone wierzcho�ki do s�ownika odwiedzonych wierzcho�k�w, ka�dy wierzcho�ek ze statusem false - tj. nie zosta� jeszcze odwiedzony
                visitDict.Add(vertex, false);
            }
            foreach (Vertex vertex in ListOfVertices)
            {
                // Iterujemy przez wszystkie wierzcho�ki, i je�eli dany wierzcho�ek nie zosta� jeszcze przeszukany to inicjujemy przeszukanie grafu (cyclic_helper) od tego momentu 
                if (!visitDict[vertex])
                {
                    if (cyclic_helper(vertex, visitDict, cyclicVertices))
                    {
                        // Je�eli na ko�cu funkcja przeszukuj�ca graf zwr�ci�a warto�� true - to znaczy �e odnalaz�a cykl, wierzcho�ki s� sortowane po ich indeksie (tj. V1, V2, V3) i przerywa p�tle
                        cyclicVertices.Sort((a, b) => a.Index.CompareTo(b.Index));
                        break;
                    }
                }
            }
            // Po sko�czeniu algorytmu przeszukania grafu wypisujemy informacj� czy graf jest cykliczny oraz wierzcho�ki kt�re tworz� cykl
            if (cyclicVertices.Count > 0)
            {
                label3.Text = "Graf jest cykliczny: \n";
                for (int i = 0; i < cyclicVertices.Count; i++)
                {
                    if (i < cyclicVertices.Count - 1)
                        label3.Text += cyclicVertices[i].Text + " -> ";
                    else
                        label3.Text += cyclicVertices[i].Text;
                }
                return true;
            }
            else
            {
                label3.Text = "Graf nie jest cykliczny!";
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            check_graph_cyclic();
        }

        // Funkcja odpowiedzalna za odbi�r sygna��w po klikni�ciu myszki w utworzone ju� wierzcho�ki
        private void vertex_MouseClick(object sender, MouseEventArgs e)
        {
            Vertex vertex = (Vertex)sender;
            // Je�eli wybrana akcja to ��czenie wierzcho�k�w to po klikni�ciu w wierzcho�ek funkcja sprawdza czy zmienna pomocnicza vertex_buffor -
            // - jest pusta, je�eli tak to znaczy �e jest to pierwszy wybrany wierzcho�ek i nie ma jak po��czy� dw�ch wierzcho�k�w
            if (action == Actions.CONNECT)
            {
                if (vertex_buffor == null)
                {
                    vertex_buffor = vertex;
                    return;
                }
                else
                {
                    // Gdy zmienna vertex_buffor nie jest pusta, to znaczy �e u�ytkownik wybra� ju� jeden wierzcho�ek i w tym momencie zosta� klikni�ty drugi 
                    // Zostaj� dodane po��czenia dla dw�ch wierzcho�k�w oraz zostaje od�wie�ony (tj. na nowo narysowany) formularz/aplikacja.
                    vertex_buffor.AddConnection(vertex);
                    vertex.AddConnection(vertex_buffor);
                    Refresh();
                    vertex_buffor = null;
                }
            }
            // Je�eli wybrana akcja to wybieranie wierzcho�k�w, to poc klini�ciu w dowolny wierzcho�ek poka�� si� wszystkie informacje na temat tego danego wierzcho�ka
            else if (action == Actions.PREVIEW)
            {
                ShowTooltip("Wierzcho�ek " + vertex.Text + " jest " + vertex.VertexDegree + " stopnia");
            }
        }
        // Pomocnicza funkcja do pokazania wybranej wiadomo�ci na ekranie w okienku informacyjnym przy kursorze
        private void ShowTooltip(string message)
        {
            new ToolTip().Show(message, this, Cursor.Position.X - Location.X, Cursor.Position.Y - Location.Y, 2000);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            // Je�eli wybrana akcja to dodawanie wierzcho�ka, to po klikni�ciu w dowolne (niezaj�te) miejsce na g��wnym panelu, utworzy si� nowy wierzcho�ek
            if (action == Actions.ADD)
            {
                Vertex vertex = new Vertex();
                vertex.Location = e.Location;
                vertex.BackColor = Color.Transparent;
                vertex.MouseClick += vertex_MouseClick;
                ListOfVertices.Add(vertex);
                int _count = ListOfVertices.Count;
                vertex.Text = "V" + _count.ToString();
                vertex.Index = _count;
                panel1.Controls.Add(vertex);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Funkcja rysuj�ca po��czenia mi�dzy wierzcho�kami
            Pen blackPen = new Pen(Color.Black, 3);
            foreach (var vertex in ListOfVertices)
            {
                foreach (var connecton in vertex.GetConnections())
                {
                    if (connecton.Index > vertex.Index)
                    {
                        e.Graphics.DrawLine(blackPen, vertex.Location, connecton.Location);
                    }
                }
            }
        }
        // Funkcje zmieniaj�ce wybran� akcj� przy pomocy RadioButton�w
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            action = Actions.ADD;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            action = Actions.CONNECT;
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            action = Actions.PREVIEW;
        }

        // Funkcja przypisana do przycisku dla generowania kodu Prufera z utworzonego grafu
        private async void button2_Click(object sender, EventArgs e)
        {
            string prufer_code = "";
            if (check_graph_cyclic())
            {
                ShowTooltip("Graf jest cykliczny - nie mo�na utworzy� kodu Prufera");
                return;
            }
            // P�tla iteruje si� ca�y czas do momentu a� ilo�� wierzcho�k�w b�dzie mniejsza ni� 3
            while (ListOfVertices.Count > 2)
            {
                // Tworzymy list� wierzcho�k�w o najmniejszej ilo�ci po��cze�/najmniejszym stopniu (1)
                List<Vertex> low_vertices = new List<Vertex>();
                foreach (var vertex in ListOfVertices)
                {
                    if (vertex.VertexDegree == 1)
                    {
                        low_vertices.Add(vertex);
                    }
                }
                // Zapisujemy pierwszy wierzcho�ek z najmniejszym stopniem do zmiennej pomocniczej
                Vertex lowest_index = low_vertices[0];
                // Dopisujemy do kodu Prufera indeks wierzcho�ka kt�ry by� po�aczony z naszym zapisanym wierzcho�kiem i nast�pnie usuwamy go z po�acze� i aplikacji
                prufer_code += lowest_index.GetConnections()[0].Index.ToString() + ", ";
                lowest_index.GetConnections()[0].RemoveConnection(lowest_index);
                ListOfVertices.Remove(lowest_index);
                Controls.Remove(lowest_index);
                panel1.Controls.Remove(lowest_index);
                lowest_index.Dispose();
                Refresh();
                await Task.Delay(500);
            }
            // Je�eli wygenerowany zosta� kod Prufera z grafu - zostaje on wypisany w odpowiednim miejscu, aplikacja czy�ci pozosta�e elementy i zostaje od�wie�ona
            if (prufer_code.Length > 0) {
                label6.Text = "Kod prufera: " + prufer_code;
                await Task.Delay(1000);
                ListOfVertices.Clear();
                panel1.Controls.Clear();
                Refresh();
            }
        }

        // Funkcja przypisana do przycisku dla generowania grafu z kodu Prufera
        private void button3_Click(object sender, EventArgs e)
        {
            ListOfVertices.Clear();
            panel1.Controls.Clear();
            Refresh();
            if (textBox1.Text.Length == 0)
            {
                return;
            }
            // Zapisujemy znaki do tablicy, bior�c je z elementu TextBox'a w kt�rym mo�emy wpisa� ci�g kodu Prufera
            string[] numbers = textBox1.Text.Split(',');
            // Tworzenie listy numer�w kodu Prufera
            List<int> prufer_numbers = new List<int>();
            for (int i = 0; i < numbers.Length; i++)
            {
                prufer_numbers.Add(int.Parse(numbers[i]));
            }
            // Tworzenie listy iteracyjnej (warto�ci od 1 do N, gdzie N to ilo�� liczb w tablicy Prufera + 2)
            List<int> iterators = new List<int>();
            for (int i = 0; i < numbers.Length + 2; i++)
            {
                iterators.Add(i + 1);
            }
            // Pomocnicze zmienne potrzebne do dzia�ania algorytmu
            int index_iterator = 0;
            bool can_add_new_vertex;
            Vertex vert1 = new Vertex();
            Vertex vert2 = new Vertex();
            // Dop�ki lista z kodem Prufera nie jest pusta, to wykonujemy algorytm prze�o�enia kodu Prufera na wierzcho�ki drzewa
            while (prufer_numbers.Count > 0)
            {
                bool is_in_prufer = false;
                for (int i = 0; i < prufer_numbers.Count; i++)
                {
                    if (iterators[index_iterator] == prufer_numbers[i])
                    {
                        is_in_prufer = true;
                        break;
                    }
                }
                if (is_in_prufer)
                {
                    index_iterator++;
                }
                else
                {
                    // Sprawdzenie wierzcho�ka dla warto�ci z listy iteracyjnej
                    can_add_new_vertex = true;
                    foreach (Vertex vert in ListOfVertices)
                    {
                        if (vert.Index == iterators[index_iterator])
                        {
                            can_add_new_vertex = false;
                            vert1 = vert;
                            break;
                        }
                    }
                    if (can_add_new_vertex)
                    {
                        vert1 = AddVertex(iterators[index_iterator]);
                    }
                    // Sprawdzenie wierzcho�ka dla warto�ci z listy kodu Prufera
                    can_add_new_vertex = true;
                    foreach (Vertex vert in ListOfVertices)
                    {
                        if (vert.Index == prufer_numbers[0])
                        {
                            can_add_new_vertex = false;
                            vert2 = vert;
                            break;
                        }
                    }
                    if (can_add_new_vertex)
                    {
                        vert2 = AddVertex(prufer_numbers[0]);
                    }
                    // Po sprawdzeniu dw�ch wierzcho�k�w (i ich opcjonalnym utworzeniem) dodajemy im wsp�lne po��czenia,
                    // od�wie�amy panel oraz usuwamy odpowiednie warto�ci z listy
                    vert1.AddConnection(vert2);
                    vert2.AddConnection(vert1);
                    Refresh();
                    iterators.RemoveAt(index_iterator);
                    prufer_numbers.RemoveAt(0);
                    index_iterator = 0;
                }
            }
            // Gdy lista z kodem Prufera jest pusta, oznacza to �e na li�cie iteracyjnej zosta� dwa elementy, kt�re r�wnie� mo�emy doda� i po��czy�
            can_add_new_vertex = true;
            foreach (Vertex vert in ListOfVertices)
            {
                if (vert.Index == iterators[0])
                {
                    can_add_new_vertex = false;
                    vert1 = vert;
                    break;
                }
            }
            if (can_add_new_vertex)
            {
                vert1 = AddVertex(iterators[0]);
            }
            can_add_new_vertex = true;
            foreach (Vertex vert in ListOfVertices)
            {
                if (vert.Index == iterators[1])
                {
                    can_add_new_vertex = false;
                    vert2 = vert;
                    break;
                }
            }
            if (can_add_new_vertex)
            {
                vert2 = AddVertex(iterators[1]);
            }
            vert1.AddConnection(vert2);
            vert2.AddConnection(vert1);
            Refresh();
            iterators.Clear();
        }
        // Funkcja tworz�ca nowy wierzcho�ek o podanym indeksie
        private Vertex AddVertex(int index)
        {
            Random random = new Random();
            Vertex vertex = new Vertex();
            // Pozycja wierzcho�ka jest generowana losowo dla koordynat�w na osi X, oraz wyliczana z indeksu dla koordynat�w na osi Y
            vertex.Location = new Point(500 + 30 * random.Next(-6, 6), 30 * index);
            vertex.BackColor = Color.Transparent;
            // Dodanie obs�ugi myszki do wierzcho�ka (aby m�c wybiera� i ��czy� go po utworzeniu)
            vertex.MouseClick += vertex_MouseClick;
            int _count = ListOfVertices.Count;
            vertex.Text = "V" + index;
            vertex.Index = index;
            // Dodanie wierzcho�ka do listy
            ListOfVertices.Add(vertex);
            // Wstawienie wierzcho�ka do sceny aplikacji
            panel1.Controls.Add(vertex);
            return vertex;
        }
        // Funkcja sprawdzaj�ca czy graf jest regularny
        private void button4_Click(object sender, EventArgs e)
        {
            if (ListOfVertices.Count == 0)
            {
                ShowTooltip("Nie mo�na sprawdzi� grafu!");
                return;
            }
            // Zmienna przechowuj�ca stopie� pierwszego wierzcho�ka na li�cie 
            int firstVertexDegree = ListOfVertices[0].VertexDegree;
            bool is_regular = true;
            // P�tla iteruj�ca wszystkie istniej�ce wierzcho�ki - je�eli kt�rykolwiek ma inny stopie� ni� zapisany to graf nie jest regularny i ko�czy si� p�tla
            foreach (var vertex in ListOfVertices)
            {
                if (vertex.VertexDegree != firstVertexDegree)
                {
                    is_regular = false;
                    break;
                }
            }
            if (is_regular)
            {
                label3.Text = "Graf jest regularny,\nka�dy wierzcho�ek jest stopnia " + firstVertexDegree.ToString();
            }
            else
            {
                label3.Text = "Graf nie jest regularny!";
            }
            
        }
    }
}
