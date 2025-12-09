using AoC.Shared;
using System.Linq;

namespace AoC._2025.Days
{
    public class Day08 : IAoCDay
    {
        public record JunctionBox(int X, int Y, int Z)
        {
            public double DistanceTo(JunctionBox coordinate)
            {
                return Math.Sqrt(Math.Pow(X - coordinate.X, 2) + Math.Pow(Y - coordinate.Y, 2) + Math.Pow(Z - coordinate.Z, 2));
            }

        }

        public class Circuit
        {
            private HashSet<JunctionBox> _junctionBoxes = new HashSet<JunctionBox>();

            public Circuit(IEnumerable<JunctionBox> junctionBoxes)
            {
                _junctionBoxes = new HashSet<JunctionBox>(junctionBoxes);
            }

            private Circuit(HashSet<JunctionBox> junctionBoxes)
            {
                _junctionBoxes = junctionBoxes;
            }

            public IEnumerable<JunctionBox> All()
            {
                foreach (var jb in _junctionBoxes)
                {
                    yield return jb;
                }
            }

            public void Add(JunctionBox junctionBox)
            {
                _junctionBoxes.Add(junctionBox);
            }

            public bool Has(JunctionBox junctionBox)
            {
                return _junctionBoxes.Contains(junctionBox);
            }

            public int Count()
            {
                return _junctionBoxes.Count();
            }

            public Circuit MergeWith(Circuit other)
            {
                return new Circuit(new HashSet<JunctionBox>(this._junctionBoxes.Union(other._junctionBoxes)));
            }
        }

        public class Circuits
        {
            private Dictionary<JunctionBox, Circuit> _circuitByJunctionBox = new();
            private Dictionary<Circuit, int> _circuitJbCounts = new();

            public void AddCircuit(Circuit circut)
            {
                var count = 0;

                foreach (JunctionBox jb in circut.All())
                {
                    count++;
                    _circuitByJunctionBox.Add(jb, circut);
                }

                _circuitJbCounts.Add(circut, count);
            }
            public int Count() => _circuitJbCounts.Count;

            public void ConnectJunctionBoxes(JunctionBox jb1, JunctionBox jb2)
            {
                if (_circuitByJunctionBox.TryGetValue(jb1, out var circuit))
                {
                    if (_circuitByJunctionBox.TryGetValue(jb2, out var circuit2))
                    {
                        if (circuit == circuit2) return;

                        var jbsInCircuit1 = circuit.All();
                        var jbsInCircuit2 = circuit2.All();

                        var newCircuit = circuit.MergeWith(circuit2);

                        foreach (var jb in jbsInCircuit2.Union(jbsInCircuit1))
                        {
                            _circuitByJunctionBox[jb] = newCircuit;
                        }

                        _circuitJbCounts.Add(newCircuit, _circuitJbCounts[circuit] + _circuitJbCounts[circuit2]);

                        _circuitJbCounts.Remove(circuit);
                        _circuitJbCounts.Remove(circuit2);
                    }
                    else
                    {
                        circuit.Add(jb2);
                        _circuitByJunctionBox.Add(jb2, circuit);
                        _circuitJbCounts[circuit] = ++_circuitJbCounts[circuit];
                    }
                }
                else if (_circuitByJunctionBox.TryGetValue(jb2, out var circuit3))
                {
                    circuit3.Add(jb1);
                    _circuitByJunctionBox.Add(jb1, circuit3);
                    _circuitJbCounts[circuit3] = ++_circuitJbCounts[circuit3];
                }
                else
                {
                    var newCircuit = new Circuit([ jb1, jb2 ]);

                    _circuitByJunctionBox.Add(jb1, newCircuit);
                    _circuitByJunctionBox.Add(jb2, newCircuit);
                    _circuitJbCounts.Add(newCircuit, 2);
                }
            }

            public List<(Circuit, int)> Get3Largest()
            {
                return _circuitJbCounts.OrderByDescending(x => x.Value).Take(3).Select(x=> (x.Key, x.Value)).ToList();
            }
        }

        public string Part1(string input)
        {
            var lines = input.Split("\n");

            var junctionBoxDistances = new List<(JunctionBox jb, JunctionBox jb2, double Distance)>();

            var junctionBoxes = new List<JunctionBox>();

            foreach (var line in lines)
            {
                var axis = line.Split(',');
                var newJunctionBox = new JunctionBox(int.Parse(axis[0]), int.Parse(axis[1]), int.Parse(axis[2]));

                foreach (var junctionBox in junctionBoxes)
                {
                    junctionBoxDistances.Add((junctionBox, newJunctionBox, junctionBox.DistanceTo(newJunctionBox)));
                }

                junctionBoxes.Add(newJunctionBox);
            }

            var shortestTen = junctionBoxDistances.OrderBy(x => x.Distance).Take(1000);

            var circuits = new Circuits();

            foreach (var shortestToConnect in shortestTen)
            {
                circuits.ConnectJunctionBoxes(shortestToConnect.jb, shortestToConnect.jb2);
            }

            var largest3Circuits = circuits.Get3Largest();

            return (largest3Circuits[0].Item2 * largest3Circuits[1].Item2 * largest3Circuits[2].Item2).ToString();
        }

        public string Part2(string input)
        {
            var lines = input.Split("\n");

            var junctionBoxDistances = new List<(JunctionBox jb, JunctionBox jb2, double Distance)>();

            var junctionBoxes = new List<JunctionBox>();
            var circuits = new Circuits();

            foreach (var line in lines)
            {
                var axis = line.Split(',');
                var newJunctionBox = new JunctionBox(int.Parse(axis[0]), int.Parse(axis[1]), int.Parse(axis[2]));

                foreach (var junctionBox in junctionBoxes)
                {
                    junctionBoxDistances.Add((junctionBox, newJunctionBox, junctionBox.DistanceTo(newJunctionBox)));
                }

                junctionBoxes.Add(newJunctionBox);

                circuits.AddCircuit(new Circuit([newJunctionBox]));
            }


            foreach (var shortestToConnect in junctionBoxDistances.OrderBy(x => x.Distance))
            {
                circuits.ConnectJunctionBoxes(shortestToConnect.jb, shortestToConnect.jb2);
                
                if (circuits.Count() == 1)
                {
                    return ((long)shortestToConnect.jb.X * shortestToConnect.jb2.X).ToString();
                }
            }

            return "0";
        }
    }
}