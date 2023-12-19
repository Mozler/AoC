// namespace DSA
// {
//     internal class DynamicArray
//     {
//         static void Main(string[] args)
//         {
//             Console.WriteLine("Hello, World!");
//             ArraySeq myArr = new ArraySeq();
//             myArr.Build(new int[] { 1, 2, 3 });
//             System.Console.WriteLine(myArr.InsertAt(1, 5));
//             System.Console.WriteLine(myArr.InsertLast(99));
//             foreach (var item in myArr)
//             {
//                 System.Console.WriteLine(item);
//             };
//         }

//         class ArraySeq
//         {
//             private int[] A;
//             public int size;
//             public int Len()
//             {
//                 return size;
//             }
//             public IEnumerator<int> GetEnumerator()
//             {
//                 for (int i = 0; i < size; i++)
//                 {
//                     yield return A[i];
//                 }
//             }
//             public ArraySeq()
//             {
//                 size = 0;
//                 A = Array.Empty<int>();
//             }

//             public void Build(int[] buildFromArr)
//             {
//                 size = buildFromArr.Length;
//                 A = new int[size];
//                 for (int i = 0; i < size; i++)
//                 {
//                     A[i] = buildFromArr[i];
//                 }
//             }
//             public int GetAt(int i)
//             {
//                 return A[i];
//             }

//             public void SetAt(int i, int v)
//             {
//                 A[i] = v;
//             }
//             public void CopyForward(int StartFrom, int ItemCount, int CopyTo, int[] Arr)
//             {
//                 for (int k = 0; k < ItemCount; k++)
//                 {
//                     Arr[CopyTo + k] = A[StartFrom + k];
//                 }
//             }
//             public void CopyBackward(int StartFrom, int ItemCount, int CopyTo, int[] Arr)
//             {
//                 for (int k = ItemCount - 1; k >= 0; k--)
//                 {
//                     Arr[CopyTo + k] = A[StartFrom + k];
//                 }
//             }

//             public int InsertAt(int i, int v)
//             {
//                 int[] Arr = new int[size + 1];
//                 CopyForward(0, i, 0, Arr);
//                 Arr[i] = v;
//                 CopyForward(i, size - i, i + 1, Arr);
//                 Build(Arr);
//                 return Arr[i];
//             }
//             public int DeleteAt(int i)
//             {
//                 int x = A[i];
//                 int[] Arr = new int[size - 1];
//                 CopyForward(0, i, 0, Arr);
//                 CopyForward(i + 1, size - i - 1, i, Arr);
//                 Build(Arr);
//                 return x;
//             }

//             public int InsertFirst(int v)
//             {
//                 return InsertAt(0, v);
//             }
//             public int DeleteFirst(int v)
//             {
//                 return DeleteAt(0);
//             }

//             public int InsertLast(int v)
//             {
//                 return InsertAt(size, v);
//             }
//             public int DeleteLast(int v)
//             {
//                 return DeleteAt(size - 1);
//             }

//         }
//     }
// }