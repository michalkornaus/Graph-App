using System.Diagnostics;

namespace GraphApp
{
    public partial class Form1 : Form
    {
        // Tablica przechowuj¹ca wszystkie istniej¹ce wierzcho³ki 
        private List<Vertex> ListOfVertices = new List<Vertex>();
        // Zmienne pomocnicze do algorytmu sprawdzaj¹cego czy graf jest cykliczny
        private Vertex? vertex_buffor = null;
        private bool found_cycle = false;
        private Vertex? last_vertex = null;
        // Zmienna Actions zawiera 3 dostêpne opcje/akcje które u¿ytkownik mo¿e wykonaæ na wierzcho³kach,
        // Domyœlnie program rozpoczyna z zaznaczon¹ opcj¹ Preview - czyli Wybieraniem wierzcho³ka
        enum Actions { PREVIEW, ADD, CONNECT }
        private Actions action = Actions.PREVIEW;
        public Form1()
        {
            InitializeComponent();
        }
        // Funkcja inicjuj¹ca przeszukanie grafu wg³¹b w poszukiwaniu cyklu - bierze obecny wierzcho³ek, zmienn¹ Dictionary i listê wierzcho³ków cyklicznych jako parametry
        private bool cyclic_helper(Vertex current, Dictionary<Vertex, bool> visited, List<Vertex> cyclicVert)
        {
            // Oznacza obecny wierzcho³ek jako odwiedzony - aby funkcja wiedzia³a kiedy skoñczyæ przeszukiwanie grafu
            visited[current] = true;
            // Przeszukiwane s¹ wszystkie po³¹czenia wierzcho³ka
            foreach (Vertex vertex in current.GetConnections())
            {
                // Sprawdzane zostaje czy dany po³¹czony wierzcho³ek by³ ju¿ wczeœniej przeszukiwany
                if (!visited[vertex])
                {
                    // Je¿eli wierzcho³ek nie by³ odwiedzany, to wywo³uje siê funkcja kontynuuj¹ca przeszukiwanie grafu od tego miejsca wg³¹b
                    if (cyclic_helper(vertex, visited, current, cyclicVert))
                    {
                        // Je¿eli powy¿sza funkcja przeszukuj¹ca graf zwróci³a wartoœæ true - to znaczy ¿e odnalaz³a cykl - dodany zostaje obecny wierzcho³ek do listy i zwracana wartoœæ true
                        if (!cyclicVert.Contains(current) && !found_cycle)
                            cyclicVert.Add(current);
                        return true;
                    }
                }
            }
            return false;
        }
        // Funkcja kontynuuj¹ca przeszukanie grafu wg³ab w poszkuwaniu cyklu - bierze jeden wiêcej parametr ni¿ powy¿sza funkcja: Vertex parent, aby wiedzieæ od którego wierzcho³ka wywo³a³a siê ta funkcja 
        private bool cyclic_helper(Vertex current, Dictionary<Vertex, bool> visited, Vertex parent, List<Vertex> cyclicVert)
        {
            // Oznacza obecny wierzcho³ek jako odwiedzony
            visited[current] = true;
            // Przeszukiwane s¹ wszystkie po³¹czenia wierzcho³ka
            foreach (Vertex vertex in current.GetConnections())
            {
                // Sprawdzane zostaje czy dany po³¹czony wierzcho³ek by³ ju¿ wczeœniej przeszukiwany
                if (!visited[vertex])
                {
                    // Je¿eli po³¹czony wierzcho³ek nie by³ odwiedzany, to wywo³uje siê funkcja kontynuuj¹ca przeszukiwanie grafu od tego miejsca wg³¹b
                    if (cyclic_helper(vertex, visited, current, cyclicVert))
                    {
                        // Je¿eli powy¿sza funkcja przeszukuj¹ca graf zwróci³a wartoœæ true - to znaczy ¿e odnalaz³a cykl - dodany zostaje obecny wierzcho³ek do listy
                        if (!cyclicVert.Contains(current) && !found_cycle)
                            cyclicVert.Add(current);
                        if (current == last_vertex)
                            // Je¿eli obecny wierzcho³ek jest równy ostatniemu wierzcho³kowi w cyklu to zmienia zmienn¹ found_cycle na true -
                            // - jest to zmienna pomocnicza aby móc wyœwietlaæ po przeszukaniu grafu odpowiednie wierzcho³ki sk³adaj¹ce siê na cykl, bez niej przez rekurencyjne podejœcie by³oby zbyt du¿o wierzcho³ków wpisanych do listy
                            found_cycle = true;
                        return true;
                    }
                }
                // Je¿eli dany po³¹czony wierzcho³ek by³ ju¿ przeszukiwany wczeœniej, to sprawdzamy czy dany wierzcho³ek nie jest wierzcho³kiem poprzedzaj¹cym przeszukiwanie
                else if (vertex != parent)
                {
                    // Je¿eli dotarliœmy do wierzcho³ka który by³ ju¿ odwiedzany i nie jest poprzedzaj¹cym wierzcho³kiem to znaczy ¿e odnaleŸliœmy cykl w grafie

                    // Dodajemy do listy cyklicznych wierzcho³ków obecny wierzcho³ek - je¿eli nie jest ju¿ na liœcie
                    if (!cyclicVert.Contains(current))
                        cyclicVert.Add(current);
                    // Zapisujemy do zmiennej pomocniczej last_vertex czyli ostatni wierzcho³ek w cyklu, tj. znaleziony po³¹czony wierzcho³ek
                    last_vertex = vertex;
                    // Zwracamy wartoœæ true, poniewa¿ cykl zosta³ znaleziony
                    return true;
                }
            }
            // Zwracamy wartoœæ false, je¿eli ka¿dy przeszukany po³¹czony wierzcho³ek nie mia³ innych po³¹czeñ
            return false;
        }

        private bool check_graph_cyclic()
        {
            found_cycle = false;
            label3.Text = "";
            if (ListOfVertices.Count < 3)
            {
                ShowTooltip("Nie mo¿na sprawdziæ cyklicznoœci grafu dla mniej ni¿ trzech wierzcho³ków!");
                return false;
            }
            // Utworzenie zmiennej Dictionary (s³ownika) - przyjmuj¹cej po³¹czone pary: wierzcho³ka ze zmienn¹ bool;
            // Dziêki temu mo¿emy mieæ listê wszystkich wierzcho³ków i odpowiadaj¹cy im stan true/false, tj. czy zosta³y ju¿ przeszukane
            Dictionary<Vertex, bool> visitDict = new Dictionary<Vertex, bool>();
            // Lista wierzcho³ków tworz¹cych cykl w grafie
            List<Vertex> cyclicVertices = new List<Vertex>();
            foreach (Vertex vertex in ListOfVertices)
            {
                // Dodajemy wszystkie utworzone wierzcho³ki do s³ownika odwiedzonych wierzcho³ków, ka¿dy wierzcho³ek ze statusem false - tj. nie zosta³ jeszcze odwiedzony
                visitDict.Add(vertex, false);
            }
            foreach (Vertex vertex in ListOfVertices)
            {
                // Iterujemy przez wszystkie wierzcho³ki, i je¿eli dany wierzcho³ek nie zosta³ jeszcze przeszukany to inicjujemy przeszukanie grafu (cyclic_helper) od tego momentu 
                if (!visitDict[vertex])
                {
                    if (cyclic_helper(vertex, visitDict, cyclicVertices))
                    {
                        // Je¿eli na koñcu funkcja przeszukuj¹ca graf zwróci³a wartoœæ true - to znaczy ¿e odnalaz³a cykl, wierzcho³ki s¹ sortowane po ich indeksie (tj. V1, V2, V3) i przerywa pêtle
                        cyclicVertices.Sort((a, b) => a.Index.CompareTo(b.Index));
                        break;
                    }
                }
            }
            // Po skoñczeniu algorytmu przeszukania grafu wypisujemy informacjê czy graf jest cykliczny oraz wierzcho³ki które tworz¹ cykl
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

        // Funkcja odpowiedzalna za odbiór sygna³ów po klikniêciu myszki w utworzone ju¿ wierzcho³ki
        private void vertex_MouseClick(object sender, MouseEventArgs e)
        {
            Vertex vertex = (Vertex)sender;
            // Je¿eli wybrana akcja to ³¹czenie wierzcho³ków to po klikniêciu w wierzcho³ek funkcja sprawdza czy zmienna pomocnicza vertex_buffor -
            // - jest pusta, je¿eli tak to znaczy ¿e jest to pierwszy wybrany wierzcho³ek i nie ma jak po³¹czyæ dwóch wierzcho³ków
            if (action == Actions.CONNECT)
            {
                if (vertex_buffor == null)
                {
                    vertex_buffor = vertex;
                    return;
                }
                else
                {
                    // Gdy zmienna vertex_buffor nie jest pusta, to znaczy ¿e u¿ytkownik wybra³ ju¿ jeden wierzcho³ek i w tym momencie zosta³ klikniêty drugi 
                    // Zostaj¹ dodane po³¹czenia dla dwóch wierzcho³ków oraz zostaje odœwie¿ony (tj. na nowo narysowany) formularz/aplikacja.
                    vertex_buffor.AddConnection(vertex);
                    vertex.AddConnection(vertex_buffor);
                    Refresh();
                    vertex_buffor = null;
                }
            }
            // Je¿eli wybrana akcja to wybieranie wierzcho³ków, to poc kliniêciu w dowolny wierzcho³ek poka¿¹ siê wszystkie informacje na temat tego danego wierzcho³ka
            else if (action == Actions.PREVIEW)
            {
                ShowTooltip("Wierzcho³ek " + vertex.Text + " jest " + vertex.VertexDegree + " stopnia");
            }
        }
        // Pomocnicza funkcja do pokazania wybranej wiadomoœci na ekranie w okienku informacyjnym przy kursorze
        private void ShowTooltip(string message)
        {
            new ToolTip().Show(message, this, Cursor.Position.X - Location.X, Cursor.Position.Y - Location.Y, 2000);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            // Je¿eli wybrana akcja to dodawanie wierzcho³ka, to po klikniêciu w dowolne (niezajête) miejsce na g³ównym panelu, utworzy siê nowy wierzcho³ek
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
            // Funkcja rysuj¹ca po³¹czenia miêdzy wierzcho³kami
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
        // Funkcje zmieniaj¹ce wybran¹ akcjê przy pomocy RadioButtonów
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
                ShowTooltip("Graf jest cykliczny - nie mo¿na utworzyæ kodu Prufera");
                return;
            }
            // Pêtla iteruje siê ca³y czas do momentu a¿ iloœæ wierzcho³ków bêdzie mniejsza ni¿ 3
            while (ListOfVertices.Count > 2)
            {
                // Tworzymy listê wierzcho³ków o najmniejszej iloœci po³¹czeñ/najmniejszym stopniu (1)
                List<Vertex> low_vertices = new List<Vertex>();
                foreach (var vertex in ListOfVertices)
                {
                    if (vertex.VertexDegree == 1)
                    {
                        low_vertices.Add(vertex);
                    }
                }
                // Zapisujemy pierwszy wierzcho³ek z najmniejszym stopniem do zmiennej pomocniczej
                Vertex lowest_index = low_vertices[0];
                // Dopisujemy do kodu Prufera indeks wierzcho³ka który by³ po³aczony z naszym zapisanym wierzcho³kiem i nastêpnie usuwamy go z po³aczeñ i aplikacji
                prufer_code += lowest_index.GetConnections()[0].Index.ToString() + ", ";
                lowest_index.GetConnections()[0].RemoveConnection(lowest_index);
                ListOfVertices.Remove(lowest_index);
                Controls.Remove(lowest_index);
                panel1.Controls.Remove(lowest_index);
                lowest_index.Dispose();
                Refresh();
                await Task.Delay(500);
            }
            // Je¿eli wygenerowany zosta³ kod Prufera z grafu - zostaje on wypisany w odpowiednim miejscu, aplikacja czyœci pozosta³e elementy i zostaje odœwie¿ona
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
            // Zapisujemy znaki do tablicy, bior¹c je z elementu TextBox'a w którym mo¿emy wpisaæ ci¹g kodu Prufera
            string[] numbers = textBox1.Text.Split(',');
            // Tworzenie listy numerów kodu Prufera
            List<int> prufer_numbers = new List<int>();
            for (int i = 0; i < numbers.Length; i++)
            {
                prufer_numbers.Add(int.Parse(numbers[i]));
            }
            // Tworzenie listy iteracyjnej (wartoœci od 1 do N, gdzie N to iloœæ liczb w tablicy Prufera + 2)
            List<int> iterators = new List<int>();
            for (int i = 0; i < numbers.Length + 2; i++)
            {
                iterators.Add(i + 1);
            }
            // Pomocnicze zmienne potrzebne do dzia³ania algorytmu
            int index_iterator = 0;
            bool can_add_new_vertex;
            Vertex vert1 = new Vertex();
            Vertex vert2 = new Vertex();
            // Dopóki lista z kodem Prufera nie jest pusta, to wykonujemy algorytm prze³o¿enia kodu Prufera na wierzcho³ki drzewa
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
                    // Sprawdzenie wierzcho³ka dla wartoœci z listy iteracyjnej
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
                    // Sprawdzenie wierzcho³ka dla wartoœci z listy kodu Prufera
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
                    // Po sprawdzeniu dwóch wierzcho³ków (i ich opcjonalnym utworzeniem) dodajemy im wspólne po³¹czenia,
                    // odœwie¿amy panel oraz usuwamy odpowiednie wartoœci z listy
                    vert1.AddConnection(vert2);
                    vert2.AddConnection(vert1);
                    Refresh();
                    iterators.RemoveAt(index_iterator);
                    prufer_numbers.RemoveAt(0);
                    index_iterator = 0;
                }
            }
            // Gdy lista z kodem Prufera jest pusta, oznacza to ¿e na liœcie iteracyjnej zosta³ dwa elementy, które równie¿ mo¿emy dodaæ i po³¹czyæ
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
        // Funkcja tworz¹ca nowy wierzcho³ek o podanym indeksie
        private Vertex AddVertex(int index)
        {
            Random random = new Random();
            Vertex vertex = new Vertex();
            // Pozycja wierzcho³ka jest generowana losowo dla koordynatów na osi X, oraz wyliczana z indeksu dla koordynatów na osi Y
            vertex.Location = new Point(500 + 30 * random.Next(-6, 6), 30 * index);
            vertex.BackColor = Color.Transparent;
            // Dodanie obs³ugi myszki do wierzcho³ka (aby móc wybieraæ i ³¹czyæ go po utworzeniu)
            vertex.MouseClick += vertex_MouseClick;
            int _count = ListOfVertices.Count;
            vertex.Text = "V" + index;
            vertex.Index = index;
            // Dodanie wierzcho³ka do listy
            ListOfVertices.Add(vertex);
            // Wstawienie wierzcho³ka do sceny aplikacji
            panel1.Controls.Add(vertex);
            return vertex;
        }
        // Funkcja sprawdzaj¹ca czy graf jest regularny
        private void button4_Click(object sender, EventArgs e)
        {
            if (ListOfVertices.Count == 0)
            {
                ShowTooltip("Nie mo¿na sprawdziæ grafu!");
                return;
            }
            // Zmienna przechowuj¹ca stopieñ pierwszego wierzcho³ka na liœcie 
            int firstVertexDegree = ListOfVertices[0].VertexDegree;
            bool is_regular = true;
            // Pêtla iteruj¹ca wszystkie istniej¹ce wierzcho³ki - je¿eli którykolwiek ma inny stopieñ ni¿ zapisany to graf nie jest regularny i koñczy siê pêtla
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
                label3.Text = "Graf jest regularny,\nka¿dy wierzcho³ek jest stopnia " + firstVertexDegree.ToString();
            }
            else
            {
                label3.Text = "Graf nie jest regularny!";
            }
            
        }
    }
}
