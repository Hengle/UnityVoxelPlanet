﻿
using MyHalp;
using UnityEngine;

namespace UnityVoxelPlanet
{
    /// <summary>
    /// VoxelPlanet class.
    /// </summary>
    public class VoxelPlanet : MyComponent
    {
        /// <summary>
        /// The planet radius in units/meters.
        /// </summary>
        [Tooltip("Must be multiple of 16!")]
        public float Radius = 8192;

        /// <summary>
        /// The initial VoxelOctree size.
        /// </summary>
        public int InitialOctreeSize = 8;

        // private
        private Octree<VoxelPlanetChunk, VoxelPlanet> _octree;

        // override `OnInit`
        protected override void OnInit()
        {
            Position = MyTransform.position;

            _octree = new Octree<VoxelPlanetChunk, VoxelPlanet>();
            _octree.Init(InitialOctreeSize, this);
        }

        // override `OnTick`
        protected override void OnTick()
        {
            if (CameraController.Current == null)
            {
                Debug.LogWarning("There is no any camera controller.");
                return;
            }

            var chunks = _octree.GetChildNodes();

            // update all chunks
            foreach (var chunk in chunks)
            {
                chunk.Update(CameraController.Current.GetPosition());
            }
        }

        // private
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            _octree.DrawDebug();
        }

        /// <summary>
        /// The planet world-space position.
        /// </summary>
        public Vector3 Position { get; set; }
    }
}