using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {

    Graphics line;
    RaycastHit rch;

    public GameObject LaserSource;
    Vector3 LaserStart;
    public GameObject LaserEnd;
    Vector3 LaserFinish;

    Vector3 normal;
    Vector3 side;
    Vector3 sideYZ;
    Vector3[] quad;
    Vector2[] uv;
    Vector3[] quadYZ;
    public Shader laserMat;

    private Mesh ml;
    private Material lmat;

    private Mesh ms;
    private Material smat;

    public Texture laserTexture;
    private Renderer laserRender;
    protected int mask;
    public bool isFriendly;
    public bool isHit;
    protected GameController gameController;
    public Laser laser;
    PlayerController player;
    protected SpawnWaves bossStatus;
    bool laserOn;
    private float laserSize;

    private void Awake()
    {
        laserSize = 0.15f;
        laserOn = true;
        if (isFriendly)
            mask = 1 << 8 | 1 << 10 | 1 << 12 | 1 << 15;
        else
            mask = 1 << 11;
        //laserRender = new Renderer();
        quad = new Vector3[4];
        quadYZ = new Vector3[4];
        uv = new Vector2[4];
        ml = new Mesh();
        lmat = new Material(laserMat);
        //lmat.color = new Color(0, 0, 0, 1.0f);

        ms = new Mesh();
        smat = new Material(laserMat);
        smat.SetTexture("_MainTex", laserTexture);
        GameObject target = GameObject.FindWithTag("GameController");
        if (target != null)
        {
            gameController = target.GetComponent<GameController>();
            bossStatus = target.GetComponent<SpawnWaves>();
        }

        if (GameObject.FindGameObjectWithTag("PlayerShip") != null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("PlayerShip");
            player = p.GetComponent<PlayerController>();
        }
        //laserRender = GetComponent<Renderer>();
        //laserRender.material.SetTexture("_MainTex", laserTexture);
        //smat.color = new Color(laserMat.GetColor("_Color").r, laserMat.GetColor("_Color").g, laserMat.GetColor("_Color").b);
    }

    private void OnEnable()
    {
        DrawQuad();
    }

    private void Update()
    {
        //always clear the previous drawing before starting on next
        ml = new Mesh();
        ms = new Mesh();

        if (!laserOn)
            return;

        smat.SetTextureOffset("_MainTex",new Vector2((smat.GetTextureOffset("_MainTex").x - Time.deltaTime * 5)%1,0.0f));

        DrawQuad();
        AddLine(ml, quad, uv, false);
        AddLine(ms, quadYZ, uv, false);
        DrawLaserBeam();

        //transform.rotation = Quaternion.identity;
        //first = null;
    }

    void DrawLaserBeam()
    {
        //Graphics.DrawMesh(ml, LaserSource.transform.position, LaserSource.transform.rotation, smat, 0);
        //Graphics.DrawMesh(ms, LaserSource.transform.position, LaserSource.transform.rotation, lmat, 0);
        Graphics.DrawMesh(ml, Matrix4x4.identity, smat, 0);
        Graphics.DrawMesh(ms, Matrix4x4.identity, smat, 0);
    }

    //Credit to Bartek Drozdz for all functions below this comment, except where comments specify otherwise
    //Reference: http://www.everyday3d.com/blog/index.php/2010/03/15/3-ways-to-draw-3d-lines-in-unity3d/
    void AddLine(Mesh m, Vector3[] quad, Vector2[] uv, bool tmp)
    {
        int vl = m.vertices.Length;

        Vector3[] vs = m.vertices;
        if (!tmp || vl == 0) vs = resizeVertices(vs, 4);
        else vl -= 4;

        vs[vl] = quad[0];
        vs[vl + 1] = quad[1];
        vs[vl + 2] = quad[2];
        vs[vl + 3] = quad[3];

        if (Physics.Linecast(quad[0], quad[2], out rch, mask, QueryTriggerInteraction.Collide) ||
            Physics.Linecast(quad[1], quad[3], out rch, mask, QueryTriggerInteraction.Collide) ||
            Physics.Linecast(quadYZ[0], quadYZ[2], out rch, mask, QueryTriggerInteraction.Collide) ||
            Physics.Linecast(quadYZ[1], quadYZ[3], out rch, mask, QueryTriggerInteraction.Collide))
        {
            HandleHit(rch.collider);
            if (!isFriendly)
                GetComponent<DestroyPlayer>().HandlePlayerHit(rch.collider);
        }

            /*Vector3[] normals = new Vector3[4];
        normals[vl] = Vector3.back;
        normals[vl + 1] = Vector3.back;
        normals[vl + 2] = Vector3.back;
        normals[vl + 3] = Vector3.back;*/

        /*int ul = m.vertices.Length;

        Vector2[] us = m.vertices;
        if (!tmp || vl == 0) vs = resizeVertices(vs, 4);
        else vl -= 4;*/

        vs[vl] = quad[0];
        vs[vl + 1] = quad[1];
        vs[vl + 2] = quad[2];
        vs[vl + 3] = quad[3];
        int tl = m.triangles.Length;

        int[] ts = m.triangles;
        if (!tmp || tl == 0) ts = resizeTraingles(ts, 6);
        else tl -= 6;
        ts[tl] = vl;
        ts[tl + 1] = vl + 1;
        ts[tl + 2] = vl + 2;
        ts[tl + 3] = vl + 1;
        ts[tl + 4] = vl + 3;
        ts[tl + 5] = vl + 2;

        m.vertices = vs;

        //added in uvs
        m.uv = uv;
        m.triangles = ts;
        //m.normals = normals;
        m.RecalculateBounds();
        m.RecalculateNormals();
    }

    void DrawQuad()
    {
        LaserStart = LaserSource.transform.position;
        LaserFinish = LaserEnd.transform.position;
        line = new Graphics();
        normal = Vector3.Cross(LaserStart, LaserFinish);
        side = Vector3.Cross(normal, LaserFinish - LaserStart);
        //sideYZ = Vector3.Cross(normal, side - LaserStart);
        side.Normalize();
        //sideYZ.Normalize();
        //Debug.Log(side);
        if (side.Equals(Vector3.zero))
        {
            side = Vector3.Cross(Vector3.forward, LaserFinish - LaserStart);
            side.Normalize();
            //sideYZ = Vector3.Cross(Vector3.right, LaserFinish - LaserStart);
            //sideYZ.Normalize();
        }
        quad[0] = LaserStart + side * laserSize;
        quad[1] = LaserStart + side * -laserSize;
        quad[2] = LaserFinish + side * laserSize;
        quad[3] = LaserFinish + side * -laserSize;
        quadYZ[0] = Quaternion.AngleAxis(90.0f, transform.up) * quad[0];
        quadYZ[1] = Quaternion.AngleAxis(90.0f, transform.up) * quad[1];
        quadYZ[2] = Quaternion.AngleAxis(90.0f, transform.up) * quad[2];
        quadYZ[3] = Quaternion.AngleAxis(90.0f, transform.up) * quad[3];/**/
        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(0, 0);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);
    }

    Vector3[] resizeVertices(Vector3[] ovs, int ns)
    {
        Vector3[] nvs = new Vector3[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
        return nvs;
    }

    int[] resizeTraingles(int[] ovs, int ns)
    {
        int[] nvs = new int[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
        return nvs;
    }

    public void ToggleLaser(bool toggle)
    {
        laserOn = toggle;
    }

    protected virtual void HandleHit(Collider other)
    {
        if (!isFriendly)
            return;

        if (other.GetComponent<BossArmorBehavior>() != null)
        {
            isHit = true;
            return;
        }

        AbstractEnemy enemy = GetComponent<OnHitHandler>().OnHitHandle(other, gameController);

        if (enemy == null)
            return;

        if (enemy.getDeathStatus())
            return;

        isHit = true;
        if (enemy.takeDamage(laser.damage) <= 0)
        {
            GetComponent<OnHitHandler>().OnHitLogic(other, gameController, enemy);
            isHit = false;
        }
        
    }

    public void ChangeLaserSize(float size)
    {
        laserSize = size;
    }
}
