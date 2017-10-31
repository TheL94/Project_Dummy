using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class Floor : CellElement
    {
        public override void SetFillerGraphic(GameObject _graphic, Vector3 _position, Quaternion _rotation)
        {
            base.SetFillerGraphic(_graphic, _position, _rotation);
            if(fillerGraphics[0] != null)
            {
                fillerGraphics[0].transform.position += new Vector3(Random.Range(0.1f, 1.8f), 0f, Random.Range(0.1f, 1.8f));
                fillerGraphics[0].transform.localScale = new Vector3(Random.Range(0.8f, 1.3f), Random.Range(0.8f, 1.3f), Random.Range(0.8f, 1.3f));
                fillerGraphics[0].transform.localEulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);

            }
        }

        public override void DestroyObject(bool _destroyComponentOnly = false)
        {
            RelativeCell.Floor = null;
            base.DestroyObject(_destroyComponentOnly);
        }
    }
}

