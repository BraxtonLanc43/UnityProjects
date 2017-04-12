using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface scrI_NPC {
       
    //Interface methods
    void faceNorth();
    void faceSouth();
    void faceWest();
    void faceEast();
    void engageCommentary();
    void playFirstCommentary();
    void playAdditionalCommentary();
    void faceDirection(Direction dir);
    bool lineOfSight_Player(Direction dir);
    void spotted();
    void walkToPlayer(Direction dir);
    void walkToSpot(Vector3 dest, Direction dir);
    void toRunNorth();
    void toRunSouth();
    void toRunEast();
    void toRunWest();
}
