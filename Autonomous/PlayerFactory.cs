using Autonomous.Public;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Autonomous.Impl.GameObjects;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Autonomous.Impl
{
    public class PlayerFactory
    {
        [ImportMany]
        IEnumerable<Lazy<IPlayer, IPlayerData>> players;

        private CompositionContainer _container;
        private Model porsheModel;
        private Model lamborginiModel;
        private Model porshe911;
        private IList<Model> carModels;
        private int carModelIndex = 0;

        public IEnumerable<IPlayerData> PlayersInfo
        {
            get
            {
                foreach (Lazy<IPlayer, IPlayerData> player in players)
                {
                    yield return player.Metadata;
                }
            }
        }

        public int HumanPlayerIndex
        {
            get
            {
                int index = 0;
                foreach (var item in PlayersInfo)
                {
                    if (item.PlayerName.Equals("Human", StringComparison.InvariantCultureIgnoreCase))
                        return index;

                    ++index;
                }

                return -1;
            }
        }

        public void LoadContent(ContentManager content)
        {
            porsheModel = content.Load<Model>("Cars/Porshe/carrgt");
            lamborginiModel = content.Load<Model>("Lambo/Lamborghini_Aventador");
            porshe911 = content.Load<Model>("Cars/Porshe911/Porsche_911_GT2");

            carModels = new List<Model>
            {
                lamborginiModel,
                porshe911,
                porsheModel
            };
        }

        public IEnumerable<Car> LoadPlayers(GameStateManager gameStateManager)
        {
            //An aggregate catalog that combines multiple catalogs  
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class  
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(PlayerFactory).Assembly));

            catalog.Catalogs.Add(new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory));

            //Create the CompositionContainer with the parts in the catalog  
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object  
            this._container.ComposeParts(this);

            int playerIndex = 0;
            foreach (var player in this.players)
            {
                var model = GetNextCar();
                var x = GameConstants.LaneWidth *1.5f - playerIndex * GameConstants.LaneWidth;
                string id = Guid.NewGuid().ToString();
                PlayerGameLoop.StartGameLoop(player.Value, id, gameStateManager);

                yield return new Car(model, id, player.Metadata.PlayerName, gameStateManager, x);
                playerIndex++;
            }
        }
        
        private Model GetNextCar()
        {
            var model = carModels[carModelIndex];
            carModelIndex = (++carModelIndex) % carModels.Count;
            return model;
        }

    }
}
