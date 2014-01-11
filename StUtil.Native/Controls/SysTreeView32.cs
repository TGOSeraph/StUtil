using StUtil.Internal.Native;
using StUtil.Native.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace StUtil.Native.Controls
{
    /// <summary>
    /// TODO: Finish
    /// </summary>
    public sealed class SysTreeView32
    {
        public IntPtr Handle { get; private set; }
        public ExternalProcess Process { get; private set; }

        public SysTreeView32(IntPtr handle)
        {
            this.Handle = handle;
            this.Process = new ExternalProcess(this.Handle);
            this.Process.Open();
        }

        public SysTreeNode32 Root
        {
            get
            {
                return new SysTreeNode32(this, null, NativeMethods.SendMessage(Handle, TVM_GETNEXTITEM, TVGN_ROOT, IntPtr.Zero));
            }
        }

        private const int TV_FIRST = 0x1100;
        private const int TVM_GETNEXTITEM = (TV_FIRST + 10);
        private const int TVM_GETITEMA = (TV_FIRST + 12);
        private const int TVM_GETITEMW = 0x113E;
        private const int TVGN_ROOT = 0x0;
        private const int TVGN_NEXT = 0x1;
        private const int TVGN_CHILD = 0x4;

        private const int TVIF_TEXT = 0x1;

        public class SysTreeNode32
        {
            public SysTreeView32 Tree { get; private set; }
            public int Handle { get; private set; }

            public SysTreeNode32 Parent { get; private set; }

            public SysTreeNode32(SysTreeView32 tree, SysTreeNode32 parent, int handle)
            {
                this.Tree = tree;
                this.Parent = parent;
                this.Handle = handle;
            }

            public SysTreeNode32 FirstChild
            {
                get
                {
                    return new SysTreeNode32(this.Tree, this.Parent, 
                        NativeMethods.SendMessage(this.Tree.Handle, TVM_GETNEXTITEM, TVGN_CHILD, this.Handle));
                }
            }

            public IEnumerable<SysTreeNode32> Children
            {
                get
                {
                    List<SysTreeNode32> nodes = new List<SysTreeNode32>();
                    SysTreeNode32 node = FirstChild;
                    while(node.Handle > 0)
                    {
                        nodes.Add(node);
                        node = node.Next;
                    }
                    return nodes;
                }
            }

            public SysTreeNode32 Next
            {
                get
                {
                    return new SysTreeNode32(this.Tree, this.Parent, 
                        NativeMethods.SendMessage(this.Tree.Handle, TVM_GETNEXTITEM, TVGN_NEXT, this.Handle));
                }
            }

            public string Text
            {
                get
                {
                    using (ExternalMemory stringMemory = this.Tree.Process.Allocate((uint)255))
                    {
                        NativeStructs.TVITEMEX tvItem = new NativeStructs.TVITEMEX();
                        tvItem.mask = TVIF_TEXT;
                        tvItem.hItem = new IntPtr(this.Handle);
                        //Set the text to point to our allocated memory
                        tvItem.pszText = stringMemory.Address;
                        tvItem.cchTextMax = 255;
                        using (ExternalMemory itemMemory = this.Tree.Process.Allocate(tvItem))
                        {
                            NativeStructs.TVITEMEX item = itemMemory.Read<NativeStructs.TVITEMEX>();


                            int success = NativeMethods.SendMessage(this.Tree.Handle, TVM_GETITEMW, 0, itemMemory.Address.ToInt32());
                            if (success != 1)
                            {
                                throw new Win32Exception();
                            }
                            return stringMemory.Read(Encoding.Unicode);
                        }
                    }
                }
            }
        }
    }
}
