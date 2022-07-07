using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class MoveToGoalAgent : Agent{
   [SerializeField] Transform targetTransform;
   [SerializeField] float moveSpeed = 1f;

   public override void OnEpisodeBegin(){
      transform.localPosition = new Vector3(Random.Range(-10,11),0,Random.Range(-10,11));
      targetTransform.localPosition = new Vector3(Random.Range(-10,11),0,Random.Range(-10,11));
   }

   public override void CollectObservations(VectorSensor sensor){
      sensor.AddObservation(transform.localPosition);
      sensor.AddObservation(targetTransform.localPosition);
   }

   public override void OnActionReceived(ActionBuffers actions){
      float moveX = actions.ContinuousActions[0];
      float moveZ = actions.ContinuousActions[1];
      transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
   }

   public override void Heuristic(in ActionBuffers actionsOut){
      ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
      continuousActions[0] = Input.GetAxisRaw("Horizontal");
      continuousActions[1] = Input.GetAxisRaw("Vertical");
   }

   void OnTriggerEnter(Collider other){
      var movementPenalty = Mathf.Max(-1f, -StepCount * 0.001f);
      if (other.CompareTag("Wall")){
         SetReward(-1f);
      }
      if(other.CompareTag("Goal")){
         SetReward(2f + movementPenalty);
      }
      EndEpisode();
   }
}
