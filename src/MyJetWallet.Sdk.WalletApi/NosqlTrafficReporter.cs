using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyNoSqlServer.DataReader;

namespace MyJetWallet.Sdk.WalletApi
{
    public class NosqlTrafficReporter
    {
        private readonly object _gate = new();
        private readonly Dictionary<string, long> _data = new();
        private DateTime _startDate = DateTime.UtcNow;


        public NosqlTrafficReporter()
        {
            MyNoSqlServerClientTcpContext.OnReceiveDataReport += ReportTraffic;
        }

        private void ReportTraffic(string table, long size)
        {
            Apply(table, size);
        }

        public void Apply(string table, long amount)
        {
            lock (_gate)
            {
                if (!_data.TryAdd(table, amount))
                    _data[table] += amount;
            }
        }
        
        public void Reset()
        {
            lock (_gate)
            {
               _data.Clear();
               _startDate = DateTime.UtcNow;
            }
        }

        public string GetReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Time: {DateTime.UtcNow - _startDate}");
            sb.AppendLine();
            lock (_gate)
            {
                foreach (var item in _data.OrderByDescending(e => e.Value))
                {
                    sb.AppendLine($"{item.Key}: {item.Value} byte");
                }
            }

            return sb.ToString();
        }
    }
}