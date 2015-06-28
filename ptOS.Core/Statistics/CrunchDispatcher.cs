using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ptOS.Core.Statistics
{
    public class CrunchDispatcher
    {
        private ptOSContainer _context = new ptOSContainer();

        public static List<IStatisticsCruncher> Crunchers = new List<IStatisticsCruncher>();

        private readonly Player _player;
        private readonly Server _server;
        private readonly StatType _type;
        public CrunchDispatcher(Player player)
        {
            _player = player;
            _type = StatType.Player;
        }

        public CrunchDispatcher(Server server)
        {
            _server = server;
            _type = StatType.Server;
        }

        public CrunchDispatcher()
        {
            _type = StatType.Other;
        }

        public CrunchDispatcher Crunch()
        {
            Task.Run(() =>
            {
                var obj = _type == StatType.Player ? (object)_player : (_type == StatType.Server ? _server : null);
                foreach (var c in Crunchers.Where(x => x.Type == _type))
                {
                    c.Crunch(_context, obj);
                }
            });
            return this;
        }

        public CrunchDispatcher Crunch(string name)
        {
            Task.Run(() =>
            {
                var obj = _type == StatType.Player ? (object)_player : (_type == StatType.Server ? _server : null);
                foreach (var c in Crunchers.Where(x => x.Type == _type && x.Name == name))
                {
                    c.Crunch(_context, obj);
                }
            });

            return this;
        }
    }
}
