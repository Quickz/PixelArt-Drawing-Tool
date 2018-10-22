using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

/**
 * Note:
 * This will have to be refactored in the future
 * to support more complex shortcut combinations.
 **/

namespace PixelArt_Drawing_Tool
{
    class ShortcutManager
    {
        private List<Key> keys = new List<Key>();
        private List<bool> useCtrl = new List<bool>();

        /// <summary>
        ///  Collection of actions that get called
        ///  when a shortcut is used.
        /// </summary>
        private List<Action> actions = new List<Action>();

        private bool ctrlDown = false;

        /// <summary>
        ///  Adds a new shortcut.
        /// </summary>
        /// <param name="key">
        ///  Key to press down to use the shortcut.
        /// </param> 
        /// <param name="useCtrl">
        ///  If true, a shift key will have to be held
        ///  down before pressing the "key".
        /// </param>
        public void Add(Key key, bool shiftDown, Action action)
        {
            keys.Add(key);
            useCtrl.Add(shiftDown);
            actions.Add(action);
        }

        public void HandleKeyDown(Key key)
        {
            if (key == Key.LeftCtrl || key == Key.RightCtrl)
            {
                ctrlDown = true;
                return;
            }

            for (int i = 0; i < keys.Count; i++)
            {
                if (key == keys[i] &&
                   (!useCtrl[i] || useCtrl[i] && ctrlDown))
                {
                    actions[i]();
                }
            }
        }

        public void HandleKeyUp(Key key)
        {
            if (key == Key.LeftCtrl || key == Key.RightCtrl)
            {
                ctrlDown = false;
            }
        }
    }
}
