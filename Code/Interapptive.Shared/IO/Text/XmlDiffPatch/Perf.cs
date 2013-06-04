//------------------------------------------------------------------------------
// <copyright file="Perf.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

#define MEASURE_PERF

namespace Microsoft.XmlDiffPatch {

#if MEASURE_PERF
    public class XmlDiffPerf 
    {
        public int loadTime = 0;
        public int hashValueComputeTime = 0;
        public int identicalOrNoDiffWriterTime = 0;
        public int matchTime = 0;
        public int preprocessTime = 0;
        public int treeDistanceTime = 0;
        public int diffgramGenerationTime = 0;
        public int diffgramSaveTime = 0;

        public int TotalTime { 
            get { 
                return loadTime + hashValueComputeTime + identicalOrNoDiffWriterTime + matchTime + preprocessTime +
                    treeDistanceTime + diffgramGenerationTime + diffgramSaveTime; 
            } 
        }

        public void Clean() 
        {
            loadTime = 0;
            hashValueComputeTime = 0;
            identicalOrNoDiffWriterTime = 0;
            matchTime = 0;
            preprocessTime = 0;
            treeDistanceTime = 0;
            diffgramGenerationTime = 0;
            diffgramSaveTime = 0;
        }
    }
#endif
}