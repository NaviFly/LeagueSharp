using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace TheCheater
{
    class TheCheater
    {
        private readonly Dictionary<int, List<IDetector>> _detectors = new Dictionary<int, List<IDetector>>();
        private Menu _mainMenu;
        private Vector2 _screenPos;

        public void Load()
        {
            _mainMenu = new Menu("The Cheater", "thecheater", true);
            var detectionType = new MenuItem("detection", "Detection").SetValue(new StringList(new[] { "Preferred", "Safe", "AntiHumanizer" }));
            detectionType.ValueChanged += (sender, args) =>
            {
                foreach (var detector in _detectors)
                {
                    detector.Value.ForEach(item => item.ApplySetting((DetectorSetting)args.GetNewValue<StringList>().SelectedIndex));
                }
            };
            _mainMenu.AddItem(detectionType);
            _mainMenu.AddItem(new MenuItem("enabled", "Enabled").SetValue(true));
            _mainMenu.AddItem(new MenuItem("drawing", "Drawing").SetValue(true));
            var posX = new MenuItem("positionx", "Position X").SetValue(new Slider(Drawing.Width - 270, 0, Drawing.Width - 20));
            var posY = new MenuItem("positiony", "Position Y").SetValue(new Slider(Drawing.Height / 2, 0, Drawing.Height - 20));
            
            posX.ValueChanged += (sender, args) => _screenPos.X = args.GetNewValue<Slider>().Value;
            posY.ValueChanged += (sender, args) => _screenPos.Y = args.GetNewValue<Slider>().Value;
            _mainMenu.AddItem(posX);
            _mainMenu.AddItem(posY);

            _mainMenu.AddToMainMenu();

            _screenPos.X = posX.GetValue<Slider>().Value;
            _screenPos.Y = posY.GetValue<Slider>().Value;


            Obj_AI_Base.OnNewPath += OnNewPath;
            Drawing.OnDraw += Draw;
    
            Notifications.AddNotification("TheCheater loaded!", 2);
        }

        private void Draw(EventArgs args)
        {
            if (!_mainMenu.Item("drawing").GetValue<bool>()) return;

            Drawing.DrawLine(new Vector2(_screenPos.X, _screenPos.Y + 15), new Vector2(_screenPos.X + 180, _screenPos.Y + 15), 2, Color.Red);
           
            var column = 1;
            Drawing.DrawText(_screenPos.X, _screenPos.Y, Color.Red, "Cheat-pattern detection:");
            foreach (var detector in _detectors)
            {
                var maxValue = detector.Value.Max(item => item.GetScriptDetections());
                
                Drawing.DrawText(_screenPos.X, column * 20 + _screenPos.Y, Color.Red, HeroManager.AllHeroes.First(hero => hero.NetworkId == detector.Key).ChampionName + ": " + maxValue + (maxValue > 0 ? " (" + detector.Value.First(itemId => itemId.GetScriptDetections() == maxValue).GetName() + ")" : string.Empty));
                column++;
            }
        }

        private void OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (sender.Type != GameObjectType.obj_AI_Hero || !_mainMenu.Item("enabled").GetValue<bool>()) return;

            if (!_detectors.ContainsKey(sender.NetworkId))
            {
                var detectors = new List<IDetector> { new SacOrbwalkerDetector(), new LeaguesharpOrbwalkDetector() };
                detectors.ForEach(detector => detector.Initialize((Obj_AI_Hero)sender));
                _detectors.Add(sender.NetworkId, detectors);
            }
            else
                _detectors[sender.NetworkId].ForEach(detector => detector.FeedData(args.Path.Last()));
        }


    }
}
