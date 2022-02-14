using System;

namespace PoseQBO.Models
{
    public static class PreCondition
    {
        public static void Requires(bool preCondition, string message = null)
        {
            if (preCondition == false)
                throw new Exception(message);
        }
    }
}
