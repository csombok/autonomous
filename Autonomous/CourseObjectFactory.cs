using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using MonoGameTry.GameObjects;
using MonoGameTry.Strategies;

namespace MonoGameTry
{
    class CourseObjectFactory
    {
        private Model treeModel;
        private Model barrierModel;
        private Model cityModel;
        private Model terrainModel;
        private Model buildingModel;

        public CourseObjectFactory()
        {
        }

        public void LoadContent(ContentManager content)
        {
            treeModel = content.Load<Model>("Tree\\fir");
            barrierModel = content.Load<Model>("barrier");
            cityModel = content.Load<Model>("City/The City");
            terrainModel = content.Load<Model>("mountain/mountains");
            buildingModel = content.Load<Model>("BuildingA");
        }

        public IEnumerable<Tree> GenerateTrees()
        {
            float offset = 180;
            for (int i = 0; i < 200; i++)
            {
                float x = i % 4 == 0 ? 8 : 9;
                float widthLeft = i % 2 == 0 ? 4f : 3.4f;
                float widthRight = i % 3 == 0 ? 3.4f : 4.5f;
                if (i < 30)
                {
                    widthLeft += 2f;
                    widthRight += 2f;
                }
                yield return new Tree(treeModel, x, i * 20f + offset, widthLeft);
                yield return new Tree(treeModel, -x, i * 20f + offset, widthRight);
            }
        }

        public IEnumerable<Barrier> GenerateBarriers()
        {
            float offset = 600;
            for (int i = 0; i < 150; i++)
            {
                yield return new Barrier(barrierModel, 6.3f, i * 1.8f + offset);
                yield return new Barrier(barrierModel, -6.3f, i * 1.8f + offset);
            }
        }

        public IEnumerable<City> GenerateCity()
        {
            float offset = 800;
            for (int i = 0; i < 10; i++)
            {
                yield return new City(cityModel, 0f, i * 450 + offset);
            }
        }

        public IEnumerable<Terrain> GenerateTerrain()
        {
            for (int i = 0; i < 50; i++)
            {
                yield return new Terrain(terrainModel, 8f, i * 200);
            }
        }

        public IEnumerable<BuildingA> GenerateBuildings()
        {
            for (int i = 0; i < 100; i++)
            {
                float offset = 500;
                float roatationLeft = i % 3 == 0 ? 90 : 180;
                float roatationRight = i % 2 == 0 ? 90 : 180;
                float x = i % 4 == 0 ? 11 : 13;
                float widthLeft = i % 2 == 0 ? 6f : 7f;
                float widthRight = i % 3 == 0 ? 7f : 6f;
                yield return new BuildingA(buildingModel, x, i * 30f + offset, roatationLeft);
                yield return new BuildingA(buildingModel, -x, i * 30f + offset, roatationRight);
            }
        }
    }
}
