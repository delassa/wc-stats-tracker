using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wc_stats_tracker.Models
{
    /// <summary>
    /// Data for a WC Run
    /// </summary>
    public class WCRun
    {
        /// <summary>
        /// Length of the Run
        /// </summary>
        private TimeSpan RunLength { get; set; }
        /// <summary>
        /// Number of characters found during the run
        /// </summary>
        private int CharactersFound { get; set; }
        /// <summary>
        /// Number of espers found during the run
        /// </summary>
        private int EspersFound { get; set; }
        /// <summary>
        /// Number of dragons killed during the run
        /// </summary>
        private int DragonsKilled { get; set; }
        /// <summary>
        /// Number of bosses killed during the run
        /// </summary>
        private int BossesKilled { get; set; }
        /// <summary>
        /// Number of checks done during the run
        /// </summary>
        private int ChecksDone { get; set; }
        /// <summary>
        /// Number of chests opened during the run
        /// </summary>
        private int ChestsOpened { get; set; }
        /// <summary>
        /// Did this run have a kt skip
        /// </summary>
        private bool DidKTSkip { get; set; }
    }
}
