namespace GraphApp
{
    public partial class Vertex : RadioButton
    {
        public int VertexDegree {  get; set; }
        public int Index {  get; set; } 
        
        private List<Vertex> ListOfConnections = new List<Vertex>();
        public void AddConnection(Vertex _vert)
        {
            if (!ListOfConnections.Contains(_vert))
            {
                ListOfConnections.Add(_vert);
                VertexDegree += 1;
            }
            else
            {
                new ToolTip().Show("Nie można połączyć połączonych już wierzchołków!", this, Cursor.Position.X - Location.X, Cursor.Position.Y - Location.Y, 2000);
            }
        }
        public void RemoveConnection(Vertex _vert)
        {
            ListOfConnections.Remove(_vert);
            VertexDegree -= 1;
        }
        public List<Vertex> GetConnections()
        {
            return ListOfConnections;
        }

        public Vertex()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
