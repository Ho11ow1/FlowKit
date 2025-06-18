/* Copyright 2025 Hollow1
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
using System.Collections.Generic;

using FlowKit.Common;

namespace FlowKit.UI
{
    namespace Utils
    {
        internal static class Easing
        {
            internal static float SetEasingFunction(float time, EasingType easing)
            {
                switch (easing)
                {
                    case EasingType.Linear:
                        return time;
                    case EasingType.Cubic:
                        return time * time * time;
                    case EasingType.EaseIn:
                        return time * time;
                    case EasingType.EaseOut:
                        return time * (2 - time);
                    case EasingType.EaseInOut:
                        return time < 0.5f ? 2 * time * time : -1 + (4 - 2 * time) * time;
                    default:
                        return time;
                }
            }
        }

        internal class AutoIncreaseList<T> : List<T> where T : new()
        {
            internal new T this[int index]
            {
                get
                {
                    while (index >= Count)
                    {
                        Add(new T());
                    }

                    return base[index];
                }

                set
                {
                    while (index >= Count)
                    {
                        Add(new T());
                    }

                    base[index] = value;
                }
            }
        }

        internal class StringAutoIncreaseList : List<string>
        {
            internal new string this[int index]
            {
                get
                {
                    while (index >= Count)
                    {
                        Add("");
                    }

                    return base[index];
                }

                set
                {
                    while (index >= Count)
                    {
                        Add("");
                    }

                    base[index] = value;
                }
            }
        }
    }
}
