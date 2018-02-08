using Autonomous.Public;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTry.GameObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MonoGameTry
{
    public class PlayerFactory
    {
        [ImportMany]
        IEnumerable<Lazy<IPlayer, IPlayerData>> players;

        private CompositionContainer _container;
        private Model porsheModel;
        private Model lamborginiModel;

        public void LoadContent(ContentManager content)
        {
            porsheModel = content.Load<Model>("Cars/Porshe/carrgt");
            lamborginiModel = content.Load<Model>("Lambo/Lamborghini_Aventador");
        }

        public IEnumerable<Car> LoadPlayers(GameStateManager gameStateManager)
        {
            //An aggregate catalog that combines multiple catalogs  
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class  
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));

            catalog.Catalogs.Add(new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory));

            //Create the CompositionContainer with the parts in the catalog  
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object  
            this._container.ComposeParts(this);

            int i = 0;
            foreach (var player in this.players)
            {
                var model = i % 2 == 0 ? lamborginiModel : porsheModel;
                var x = 3f + i * 2f;
                string id = Guid.NewGuid().ToString();
                PlayerGameLoop.StartGameLoop(player.Value, id, gameStateManager);
                yield return new Car(model, id, gameStateManager, x);
                i++;
            }

        }

    }
}
